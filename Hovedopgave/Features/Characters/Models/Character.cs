using System.ComponentModel.DataAnnotations.Schema;
using Hovedopgave.Features.Account.Models;
using Hovedopgave.Features.Campaigns.Models;
using Hovedopgave.Features.Photos.Models;

namespace Hovedopgave.Features.Characters.Models;

public class Character
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required CharacterRace Race { get; set; }
    public required CharacterClass Class { get; set; }
    public string Backstory { get; set; } = "";
    public required bool IsRetired { get; set; }

    public required string UserId { get; set; }
    public required User User { get; set; }

    public required string CampaignId { get; set; }
    public required Campaign Campaign { get; set; }

    public string? PhotoId { get; set; }
    public Photo? Photo { get; set; }
    
    [NotMapped]
    public CharacterProgression Progression { get; set; } = new CharacterProgression();
}