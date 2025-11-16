using FluentAssertions;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Characters.DTOs;
using Hovedopgave.Features.Characters.Models;
using Hovedopgave.Features.Characters.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration;

public class CharacterTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_Should_ReturnSuccess_When_CharacterIsCreated()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var testUserAccessor = scope.ServiceProvider.GetRequiredService<IUserAccessor>() as TestUserAccessor;
        var charactersService = scope.ServiceProvider.GetRequiredService<ICharactersService>();

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
}