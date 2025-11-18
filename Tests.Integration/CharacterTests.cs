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
}