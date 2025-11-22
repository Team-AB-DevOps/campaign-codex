using Serilog.Events;

namespace Hovedopgave.Features.Characters.Models;

/// <summary>
/// Handles character level and experience progression based on D&D 5e rules.
/// </summary>
public class CharacterProgression
{
    /// <summary>
    /// Experience points required for each level (index = level, value = cumulative XP needed)
    /// Based on D&D 5e progression table
    /// </summary>
    private static readonly int[] XpThresholds =
    [
        0,      // Level 1
        300,    // Level 2
        900,    // Level 3
        2700,   // Level 4
        6500,   // Level 5
        14000,  // Level 6
        23000,  // Level 7
        34000,  // Level 8
        48000,  // Level 9
        64000,  // Level 10
    ];

    public const int MaxLevel = 10;

    public int Level { get; private set; }
    public int ExperiencePoints { get; private set; }

    public CharacterProgression()
    {
        Level = 1;
        ExperiencePoints = 0;
    }

    /// <summary>
    /// Adds experience points and automatically handles level ups.
    /// </summary>
    /// <param name="xp">Amount of experience to add</param>
    /// <returns>The number of levels gained</returns>
    public int AddExperience(int xp)
    {
        if (xp < 0)
        {
            throw new ArgumentException("Cannot add negative experience", nameof(xp));
        }

        if (Level >= MaxLevel)
        {
            return 0; // Already max level, no XP gain matters
        }

        ExperiencePoints += xp;

        int levelsGained = 0;
        while (CanLevelUp())
        {
            LevelUp();
            levelsGained++;
        }

        return levelsGained;
    }

    public int RemoveExperience(int xp)
    {
        if (xp < 0)
        {
            throw new ArgumentException("Cannot remove negative experience", nameof(xp));
        }

        ExperiencePoints -= xp;
        if (ExperiencePoints < 0)
        {
            ExperiencePoints = 0;
        }

        // Adjust level down if necessary
        int levelsLost = 0;
        while (CanLevelDown())
        {
            Level--;
            levelsLost++;
        }

        return levelsLost;
    }

    /// <summary>
    /// Checks if the character has enough XP to level up.
    /// </summary>
    private bool CanLevelUp()
    {
        if (Level >= MaxLevel)
        {
            return false;
        }

        return ExperiencePoints >= XpThresholds[Level];
    }

    private bool CanLevelDown()
    {
        if (Level == 1)
        {
            return false;
        }

        return ExperiencePoints < XpThresholds[Level - 1];
    }

    /// <summary>
    /// Increases the character level by 1 if requirements are met.
    /// </summary>
    /// <returns>True if level up was successful</returns>
    private void LevelUp()
    {
        Level++;
    }

    /// <summary>
    /// Gets the progress percentage towards the next level.
    /// </summary>
    public double GetProgressToNextLevel()
    {
        if (Level >= MaxLevel)
        {
            return 100.0;
        }

        int currentLevelXp = Level > 1 ? XpThresholds[Level - 1] : 0;
        int nextLevelXp = XpThresholds[Level];
        int xpInCurrentLevel = ExperiencePoints - currentLevelXp;
        int xpNeededForLevel = nextLevelXp - currentLevelXp;

        return (double)xpInCurrentLevel / xpNeededForLevel * 100.0;
    }
}

