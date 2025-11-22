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

    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(150, 1)]
    [InlineData(298, 1)]
    [InlineData(299, 1)]
    [InlineData(300, 2)]
    [InlineData(301, 2)]
    [InlineData(600, 2)]
    [InlineData(898, 2)]
    [InlineData(899, 2)]
    [InlineData(900, 3)]
    [InlineData(901, 3)]
    [InlineData(1700, 3)]
    [InlineData(2698, 3)]
    [InlineData(2699, 3)]
    [InlineData(2700, 4)]
    [InlineData(2701, 4)]
    [InlineData(4500, 4)]
    [InlineData(6498, 4)]
    [InlineData(6499, 4)]
    [InlineData(6500, 5)]
    [InlineData(6501, 5)]
    [InlineData(9000, 5)]
    [InlineData(13998, 5)]
    [InlineData(13999, 5)]
    [InlineData(14000, 6)]
    [InlineData(14001, 6)]
    [InlineData(17000, 6)]
    [InlineData(22998, 6)]
    [InlineData(22999, 6)]
    [InlineData(23000, 7)]
    [InlineData(23001, 7)]
    [InlineData(28000, 7)]
    [InlineData(33998, 7)]
    [InlineData(33999, 7)]
    [InlineData(34000, 8)]
    [InlineData(34001, 8)]
    [InlineData(41000, 8)]
    [InlineData(47998, 8)]
    [InlineData(47999, 8)]
    [InlineData(48000, 9)]
    [InlineData(48001, 9)]
    [InlineData(55000, 9)]
    [InlineData(63998, 9)]
    [InlineData(63999, 9)]
    [InlineData(64000, 10)]
    [InlineData(64001, 10)]
    [InlineData(72000, 10)]
    [Theory]
    public void LevelThresholdShouldBeCorrect(int xp, int level)
    {
        var progression = new CharacterProgression();

        progression.AddExperience(xp);

        progression.Level.Should().Be(level);
    }

    [Fact]
    public void Progression_Should_startAtLevelOne()
    {
        var progression = new CharacterProgression();
        progression.Level.Should().Be(1);
        progression.ExperiencePoints.Should().Be(0);
    }


}
