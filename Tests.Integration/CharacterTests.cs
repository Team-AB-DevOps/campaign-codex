using FluentAssertions;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Characters.DTOs;
using Hovedopgave.Features.Characters.Models;
using Hovedopgave.Features.Characters.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration.Infrastructure;

namespace Tests.Integration;

public class CharacterTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_Should_ReturnSuccess_When_CharacterIsCreated()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        var characterDto = new CreateCharacterDto
        {
            Name = "Fancy Pants",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "This is backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        // Act
        var result = await charactersService.CreateCharacter(characterDto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetCharactersForCampaign_Should_ReturnCharacter_When_CharacterIsAddedToCampaign()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();

        var createDto = new CreateCharacterDto
        {
            Name = "Test Character",
            Race = CharacterRace.Dwarf,
            Class = CharacterClass.Fighter,
            Backstory = "Test backstory",
            UserId = userAccessor.GetUserId(),
            CampaignId = campaign.Id
        };

        await charactersService.CreateCharacter(createDto);

        // Act
        var result = await charactersService.GetCharactersForCampaign(campaign.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Contain(c => c.Name == "Test Character");
    }

    [Fact]
    public async Task UpdateCharacter_Should_ReturnSuccess_When_CharacterIsUpdated()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        var createDto = new CreateCharacterDto
        {
            Name = "Character to Update",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "Original backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        var updatedCharacterDto = new CharacterDto
        {
            Id = characterId,
            Name = "Updated name",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "Updated backstory",
            UserId = userId,
            CampaignId = campaign.Id,
            IsRetired = false
        };

        // Act
        var result = await charactersService.UpdateCharacter(updatedCharacterDto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify the character was actually updated
        var updatedCharacter = await DbContext.Characters.FindAsync(characterId);
        updatedCharacter.Should().NotBeNull();
        updatedCharacter.Name.Should().Be("Updated name");
        updatedCharacter.Backstory.Should().Be("Updated backstory");
    }

    [Fact]
    public async Task DeleteCharacter_Should_ReturnSuccess_When_CharacterIsDeleted()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();

        var createDto = new CreateCharacterDto
        {
            Name = "Character to Delete",
            Race = CharacterRace.Elf,
            Class = CharacterClass.Wizard,
            Backstory = "Will be deleted",
            UserId = userAccessor.GetUserId(),
            CampaignId = campaign.Id
        };
        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        // Act
        var result = await charactersService.DeleteCharacter(characterId);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    #region CreateCharacter Tests

    [Fact]
    public async Task CreateCharacter_Should_ReturnFailure_When_CampaignNotFound()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var userId = userAccessor.GetUserId();

        var characterDto = new CreateCharacterDto
        {
            Name = "Test Character",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "Test backstory",
            UserId = userId,
            CampaignId = "non-existent-campaign-id"
        };

        // Act
        var result = await charactersService.CreateCharacter(characterDto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to find campaign");
    }

    [Fact]
    public async Task CreateCharacter_Should_ReturnFailure_When_UserAlreadyHasActiveCharacter()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        var firstCharacterDto = new CreateCharacterDto
        {
            Name = "First Character",
            Race = CharacterRace.Human,
            Class = CharacterClass.Fighter,
            Backstory = "First backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        await charactersService.CreateCharacter(firstCharacterDto);

        var secondCharacterDto = new CreateCharacterDto
        {
            Name = "Second Character",
            Race = CharacterRace.Elf,
            Class = CharacterClass.Wizard,
            Backstory = "Second backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        // Act
        var result = await charactersService.CreateCharacter(secondCharacterDto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("User already has an active character in campaign");
    }

    #endregion

    #region UpdateCharacter Tests

    [Fact]
    public async Task UpdateCharacter_Should_ReturnFailure_When_CharacterNotFound()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        var updateDto = new CharacterDto
        {
            Id = "non-existent-character-id",
            Name = "Updated Name",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "Updated backstory",
            UserId = userId,
            CampaignId = campaign.Id,
            IsRetired = false
        };

        // Act
        var result = await charactersService.UpdateCharacter(updateDto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Character not found");
    }

    [Fact]
    public async Task UpdateCharacter_Should_ReturnFailure_When_UserIsNotOwner()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        // Create a character with the current user
        var createDto = new CreateCharacterDto
        {
            Name = "Owned Character",
            Race = CharacterRace.Human,
            Class = CharacterClass.Fighter,
            Backstory = "Original backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        // Get a different user's ID from the database
        var differentUser = await DbContext.Users.Where(u => u.Id != userId).FirstAsync();

        // Try to update with a different user ID
        var updateDto = new CharacterDto
        {
            Id = characterId,
            Name = "Updated Name",
            Race = CharacterRace.Human,
            Class = CharacterClass.Fighter,
            Backstory = "Updated backstory",
            UserId = differentUser.Id, // Different user ID
            CampaignId = campaign.Id,
            IsRetired = false
        };

        // Act
        var result = await charactersService.UpdateCharacter(updateDto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("You are not the characters owner");
    }

    [Fact]
    public async Task UpdateCharacter_Should_ClearPhoto_When_PhotoIdIsSetToNull()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        // Create a character first
        var createDto = new CreateCharacterDto
        {
            Name = "Character With Photo",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "Backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        // Update character without photo (PhotoId = null)
        var updateDto = new CharacterDto
        {
            Id = characterId,
            Name = "Updated Character",
            Race = CharacterRace.Human,
            Class = CharacterClass.Barbarian,
            Backstory = "Updated backstory",
            UserId = userId,
            CampaignId = campaign.Id,
            IsRetired = false,
            PhotoId = null
        };

        // Act
        var result = await charactersService.UpdateCharacter(updateDto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updatedCharacter = await DbContext.Characters
            .Include(c => c.Photo)
            .FirstOrDefaultAsync(c => c.Id == characterId);
        updatedCharacter!.PhotoId.Should().BeNull();
        updatedCharacter.Photo.Should().BeNull();
    }

    #endregion

    #region DeleteCharacter Tests

    [Fact]
    public async Task DeleteCharacter_Should_ReturnFailure_When_CharacterNotFound()
    {
        // Arrange
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();

        // Act
        var result = await charactersService.DeleteCharacter("non-existent-character-id");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Character not found");
    }

    [Fact]
    public async Task DeleteCharacter_Should_ReturnFailure_When_UserIsNotOwner()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        // Create a character with the current user
        var createDto = new CreateCharacterDto
        {
            Name = "Character to Delete",
            Race = CharacterRace.Human,
            Class = CharacterClass.Fighter,
            Backstory = "Will try to delete",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        // Manually change the character's owner to a different user
        var differentUser = await DbContext.Users.Where(u => u.Id != userId).FirstAsync();
        var character = await DbContext.Characters.FindAsync(characterId);
        character!.UserId = differentUser.Id;
        await DbContext.SaveChangesAsync();

        // Act
        var result = await charactersService.DeleteCharacter(characterId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("You are not the characters owner");
    }

    #endregion

    #region RetireCharacter Tests

    [Fact]
    public async Task RetireCharacter_Should_ReturnSuccess_When_CharacterIsRetired()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        var createDto = new CreateCharacterDto
        {
            Name = "Character to Retire",
            Race = CharacterRace.Human,
            Class = CharacterClass.Paladin,
            Backstory = "Ready to retire",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        // Act
        var result = await charactersService.RetireCharacter(characterId);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var retiredCharacter = await DbContext.Characters.FindAsync(characterId);
        retiredCharacter!.IsRetired.Should().BeTrue();
    }

    [Fact]
    public async Task RetireCharacter_Should_ReturnFailure_When_CharacterNotFound()
    {
        // Arrange
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();

        // Act
        var result = await charactersService.RetireCharacter("non-existent-character-id");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Character not found");
    }

    [Fact]
    public async Task RetireCharacter_Should_ReturnFailure_When_UserIsNotOwner()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        // Create a character with the current user
        var createDto = new CreateCharacterDto
        {
            Name = "Character to Retire Unauthorized",
            Race = CharacterRace.Dwarf,
            Class = CharacterClass.Cleric,
            Backstory = "Will try to retire without permission",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(createDto);
        var characterId = createResult.Value!;

        // Manually change the character's owner to a different user
        var differentUser = await DbContext.Users.Where(u => u.Id != userId).FirstAsync();
        var character = await DbContext.Characters.FindAsync(characterId);
        character!.UserId = differentUser.Id;
        await DbContext.SaveChangesAsync();

        // Act
        var result = await charactersService.RetireCharacter(characterId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("You are not the characters owner");
    }

    [Fact]
    public async Task RetireCharacter_Should_AllowCreatingNewCharacter_AfterRetiring()
    {
        // Arrange
        var userAccessor = ServiceProvider.GetRequiredService<IUserAccessor>();
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = userAccessor.GetUserId();

        // Create first character
        var firstCharacterDto = new CreateCharacterDto
        {
            Name = "First Character",
            Race = CharacterRace.Human,
            Class = CharacterClass.Fighter,
            Backstory = "First backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        var createResult = await charactersService.CreateCharacter(firstCharacterDto);
        var firstCharacterId = createResult.Value!;

        // Retire the first character
        await charactersService.RetireCharacter(firstCharacterId);

        // Create second character
        var secondCharacterDto = new CreateCharacterDto
        {
            Name = "Second Character",
            Race = CharacterRace.Elf,
            Class = CharacterClass.Wizard,
            Backstory = "Second backstory",
            UserId = userId,
            CampaignId = campaign.Id
        };

        // Act
        var result = await charactersService.CreateCharacter(secondCharacterDto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    #endregion
}