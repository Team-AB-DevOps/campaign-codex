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

    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-1000)]
    [Theory]
    public void RemoveExperience_Should_ThrowException_WithNegativeXp(int xp)
    {
        var progression = new CharacterProgression();

        var act = () => progression.RemoveExperience(xp);

        act.Should().Throw<ArgumentException>();
    }

    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [Theory]
    public void RemoveExperience_Should_NotThrow_WithValidXp(int xp)
    {
        var progression = new CharacterProgression();
        progression.AddExperience(500);

        var act = () => progression.RemoveExperience(xp);

        act.Should().NotThrow();
    }

    #region White box tests

    [InlineData(0, 500)]
    [InlineData(1, 499)]
    [InlineData(100, 400)]
    [InlineData(250, 250)]
    [InlineData(499, 1)]
    [InlineData(500, 0)]
    [Theory]
    public void RemoveExperience_Should_DecreaseXpCorrectly(int xpToRemove, int expectedXp)
    {
        var progression = new CharacterProgression();
        progression.AddExperience(500);

        progression.RemoveExperience(xpToRemove);

        progression.ExperiencePoints.Should().Be(expectedXp);
    }

    [InlineData(600)]
    [InlineData(1000)]
    [InlineData(10000)]
    [Theory]
    public void RemoveExperience_Should_NotGoBelowZero(int xpToRemove)
    {
        var progression = new CharacterProgression();
        progression.AddExperience(500);

        progression.RemoveExperience(xpToRemove);

        progression.ExperiencePoints.Should().Be(0);
        progression.Level.Should().Be(1);
    }


    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1000)]
    [Theory]
    public void AddExperience_Should_Return0_WhenAlreadyMaxLevel(int xp)
    {
        var progression = new CharacterProgression();
        progression.AddExperience(64000); // Max level

        var levelsGained = progression.AddExperience(xp);

        levelsGained.Should().Be(0);
        progression.Level.Should().Be(10);
    }

    // Multiple level gains test
    [InlineData(900, 2)]   // Level 1 -> 3
    [InlineData(2700, 3)]  // Level 1 -> 4
    [InlineData(64000, 9)] // Level 1 -> 10
    [Theory]
    public void AddExperience_Should_HandleMultipleLevelGains(int xp, int expectedLevelsGained)
    {
        var progression = new CharacterProgression();

        var levelsGained = progression.AddExperience(xp);

        levelsGained.Should().Be(expectedLevelsGained);
    }

    #endregion

    #region Other tests

    [Fact]
    public void GetProgressToNextLevel_Should_Return0_AtLevelStart()
    {
        var progression = new CharacterProgression();

        var progress = progression.GetProgressToNextLevel();

        progress.Should().Be(0.0);
    }

    [Fact]
    public void GetProgressToNextLevel_Should_Return50_AtMidpoint()
    {
        var progression = new CharacterProgression();
        progression.AddExperience(150); // Midpoint between 0 and 300

        var progress = progression.GetProgressToNextLevel();

        progress.Should().Be(50.0);
    }

    [InlineData(0, 0.0)]
    [InlineData(1, 0.33)]
    [InlineData(149, 49.67)]
    [InlineData(150, 50.0)]
    [InlineData(299, 99.67)]
    [Theory]
    public void GetProgressToNextLevel_Should_CalculateCorrectly_Level1(int xp, double expectedProgress)
    {
        var progression = new CharacterProgression();
        progression.AddExperience(xp);

        var progress = progression.GetProgressToNextLevel();

        progress.Should().BeApproximately(expectedProgress, 0.01);
    }

    [InlineData(300, 0.0)]    // Start of level 2
    [InlineData(301, 0.17)]   // Just above threshold
    [InlineData(599, 49.83)]  // Just below midpoint
    [InlineData(600, 50.0)]   // Midpoint
    [InlineData(898, 99.67)]  // Just below next level
    [InlineData(899, 99.83)]  // One below next level
    [Theory]
    public void GetProgressToNextLevel_Should_CalculateCorrectly_Level2(int xp, double expectedProgress)
    {
        var progression = new CharacterProgression();
        progression.AddExperience(xp);

        var progress = progression.GetProgressToNextLevel();

        progress.Should().BeApproximately(expectedProgress, 0.01);
    }

    [Fact]
    public void GetProgressToNextLevel_Should_Return100_AtMaxLevel()
    {
        var progression = new CharacterProgression();
        progression.AddExperience(64000); // Max level

        var progress = progression.GetProgressToNextLevel();

        progress.Should().Be(100.0);
    }

    #endregion
}
