using Hovedopgave.Features.Account.Models;
using Hovedopgave.Features.Campaigns.Models;
using Hovedopgave.Features.Notes.Models;
using Hovedopgave.Features.Wiki.Models;
using Microsoft.AspNetCore.Identity;

namespace Hovedopgave.Core.Data;

public class DbInitializer
{
    public static async Task SeedData(AppDbContext context, UserManager<User> userManager)
    {
        var users = new List<User>
        {
            new() { DisplayName = "Brian", UserName = "brian@test.com", Email = "brian@test.com" },
            new() { DisplayName = "Frederik", UserName = "frederik@test.com", Email = "frederik@test.com" },
            new() { DisplayName = "Inese", UserName = "inese@test.com", Email = "inese@test.com" },
            new() { DisplayName = "Danny", UserName = "danny@test.com", Email = "danny@test.com" },
        };

        if (!userManager.Users.Any())
        {
            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }

        var campaigns = new List<Campaign>
        {
            new()
            {
                DungeonMaster = users[0],
                Name = "Shattered Realm",
                Users = [users[1], users[2]]
            },
            new()
            {
                DungeonMaster = users[1],
                Name = "Storm Watch",
                Users = [users[0]]
            },
            new()
            {
                DungeonMaster = users[2],
                Name = "Ironbound"
            }
        };

        if (!context.Campaigns.Any())
        {
            await context.Campaigns.AddRangeAsync(campaigns);
        }



        if (!context.Notes.Any())
        {
            var notes = new List<Note>
            {
                new()
                {
                    Content = "<p>Notes for Campaign 1 - Bob</p>",
                    Campaign = campaigns[0],
                    CampaignId = campaigns[0].Id,
                    User = users[0],
                    UserId = users[0].Id
                },
                new()
                {
                    Content = "<p>Notes for Campaign 1 - Tom</p>",
                    Campaign = campaigns[0],
                    CampaignId = campaigns[0].Id,
                    User = users[1],
                    UserId = users[1].Id
                },
                new()
                {
                    Content = "<p>Notes for Campaign 1 - Jane</p>",
                    Campaign = campaigns[0],
                    CampaignId = campaigns[0].Id,
                    User = users[2],
                    UserId = users[2].Id
                },
                new()
                {
                    Content = "<p>Notes for Campaign 2 - Tom</p>",
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id,
                    User = users[1],
                    UserId = users[1].Id
                },
                new()
                {
                    Content = "<p>Notes for Campaign 2 - Bob</p>",
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id,
                    User = users[0],
                    UserId = users[0].Id
                },
                new()
                {
                    Content = "<p>Notes for Campaign 3 - Jane</p>",
                    Campaign = campaigns[2],
                    CampaignId = campaigns[2].Id,
                    User = users[2],
                    UserId = users[2].Id
                },
            };

            if (!context.Notes.Any())
            {
                await context.Notes.AddRangeAsync(notes);
            }
        }

        if (!context.WikiEntries.Any())
        {
            var wikiEntries = new List<WikiEntry>
            {
                new()
                {
                    Name = "The Shattered Spire",
                    Type = WikiEntryType.Location,
                    Content = "<p>A towering crystalline structure that pierces the sky, broken at its peak during the Cataclysm. The Spire is said to contain ancient magical artifacts and serves as a beacon visible for miles around.</p>",
                    IsVisible = true,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "Eldric the Wise",
                    Type = WikiEntryType.Npc,
                    Content = "<p>An elderly wizard who serves as the party's mentor and guide. Eldric possesses vast knowledge of the realm's history and the events leading to the Cataclysm. He operates from a small tower on the outskirts of Silverkeep.</p>",
                    IsVisible = true,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "The Cataclysm",
                    Type = WikiEntryType.Lore,
                    Content = "<p>Fifty years ago, a magical catastrophe tore through the realm, shattering the great Spire and fragmenting the land into floating islands. The cause remains unknown, but whispers speak of a powerful artifact misused by ancient mages.</p>",
                    IsVisible = true,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "Silverkeep",
                    Type = WikiEntryType.Location,
                    Content = "<p>The last major city-state still standing after the Cataclysm. Built on one of the largest stable land masses, it serves as a hub for adventurers and refugees. The city is ruled by the Council of Seven.</p>",
                    IsVisible = true,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "Restore the Spire",
                    Type = WikiEntryType.Quest,
                    Content = "<p>The party has been tasked with finding the three Crystal Shards scattered across the realm. These shards are believed to be the key to repairing the Spire and potentially reversing the Cataclysm's effects.</p>",
                    IsVisible = false,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "Void Blade",
                    Type = WikiEntryType.Item,
                    Content = "<p>A legendary sword forged from a metal that fell from the sky during the Cataclysm. The blade seems to absorb light and grants its wielder enhanced speed and the ability to briefly phase through solid matter.</p>",
                    IsVisible = false,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "The Shadow Cult",
                    Type = WikiEntryType.Other,
                    Content = "<p>A mysterious organization that emerged after the Cataclysm. They worship the Void and seek to prevent the restoration of the Spire, believing the current fractured state of the realm is 'the natural order'.</p>",
                    IsVisible = true,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                },
                new()
                {
                    Name = "Liara Swiftwind",
                    Type = WikiEntryType.Npc,
                    Content = "<p>A skilled ranger and tracker who operates in the Fractured Wastes. She has extensive knowledge of the dangerous territories between the floating islands and can navigate the unstable pathways better than anyone.</p>",
                    IsVisible = true,
                    Campaign = campaigns[1],
                    CampaignId = campaigns[1].Id
                }
            };

            await context.WikiEntries.AddRangeAsync(wikiEntries);
        }

        await context.SaveChangesAsync();
    }
}
