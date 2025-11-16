using FluentAssertions;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Characters.DTOs;
using Hovedopgave.Features.Characters.Models;
using Hovedopgave.Features.Characters.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration.Infrastructure;

namespace Tests.Integration;

[Collection("Integration")]
public class CharacterTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_Should_ReturnSuccess_When_CharacterIsCreated()
    {
        // Arrange
        var testUserAccessor = ServiceProvider.GetRequiredService<IUserAccessor>() as TestUserAccessor;
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();

        var campaign = await DbContext.Campaigns.FirstAsync();
        var userId = testUserAccessor!.UserId;

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
    }

    [Fact]
    public async Task GetCharactersForCampaign_Should_ReturnCharacter_When_CharacterIsAddedToCampaign()
    {
        // Arrange
        var testUserAccessor = ServiceProvider.GetRequiredService<IUserAccessor>() as TestUserAccessor;
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var campaign = await DbContext.Campaigns.FirstAsync();

        // Act
        var result = await charactersService.GetCharactersForCampaign(campaign.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DeleteCharacter_Should_ReturnSuccess_When_CharacterIsDeleted()
    {
        // Arrange
        var testUserAccessor = ServiceProvider.GetRequiredService<IUserAccessor>() as TestUserAccessor;
        var charactersService = ServiceProvider.GetRequiredService<ICharactersService>();
        var character = await DbContext.Characters
            .Include(x => x.User)
            .FirstAsync();

        // Act
        var result = await charactersService.DeleteCharacter(character.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}