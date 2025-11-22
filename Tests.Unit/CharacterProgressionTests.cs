using FluentAssertions;
using Hovedopgave.Features.Characters.Models;

namespace Tests.Unit;

public class CharacterProgressionTests
{

    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32000)]
    [InlineData(63999)]
    [InlineData(64000)]
    [InlineData(64001)]
    [Theory]
    public void XpShouldBeValid(int xp)
    {
        var progression = new CharacterProgression();

        var act = () => progression.AddExperience(xp);
        act.Should().NotThrow();
    }

    [InlineData(64001)]
    [InlineData(64002)]
    [InlineData(100000)]
    [Theory]
    public void AddingXpAboveThresholdShouldKeepLevelAtMax(int xp)
    {
        var progression = new CharacterProgression();

        progression.AddExperience(xp);

        progression.Level.Should().Be(10);
    }

    [InlineData(-2)]
    [InlineData(-1)]
    [InlineData(-1000)]
    [Theory]
    public void NegativeXpShouldThrowError(int xp)
    {
        var progression = new CharacterProgression();

        var act = () => progression.AddExperience(xp);
        act.Should().Throw();
    }
    
    [Fact]
    public void Progression_Should_startAtLevelOne()
    {
        var progression = new CharacterProgression();
        progression.Level.Should().Be(1);
        progression.ExperiencePoints.Should().Be(0);
    }
    
    
}
