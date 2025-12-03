using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ganss.Xss;
using Hovedopgave.Core.Data;
using Hovedopgave.Core.Interfaces;
using Hovedopgave.Core.Results;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Campaigns.DTOs;
using Hovedopgave.Features.Campaigns.Models;
using Hovedopgave.Features.Notes.Services;
using Microsoft.EntityFrameworkCore;

namespace Hovedopgave.Features.Campaigns.Services;

public class CampaignService(
    AppDbContext context,
    IUserAccessor userAccessor,
    IMapper mapper,
    ICloudinaryService cloudinaryService,
    INotesService notesService) : ICampaignService
{
    public async Task<List<CampaignDto>> GetAllUserCampaigns()
    {
        var user = await userAccessor.GetUserAsync();

        var campaigns = await context.Campaigns
            .Where(x => x.DungeonMaster.Id == user.Id || x.Users.Any(u => u.Id == user.Id))
            .ProjectTo<CampaignDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return campaigns;
    }

    public async Task<Result<CampaignDto>> GetCampaign(string id)
    {
        var user = await userAccessor.GetUserAsync();

        var result = await context.Campaigns
            .Where(x => x.Id == id && (x.DungeonMaster.Id == user.Id || x.Users.Any(u => u.Id == user.Id)))
            .ProjectTo<CampaignDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return result == null
            ? Result<CampaignDto>.Failure($"Failed to find campaign with id: {id} (or you are not DM/player)", 400)
            : Result<CampaignDto>.Success(result);
    }

    public async Task<Result<string>> DeleteCampaign(string id)
    {
        var user = await userAccessor.GetUserAsync();

        var campaign = await context.Campaigns
            .Where(x => x.Id == id && x.DungeonMaster.Id == user.Id)
            .Include(x => x.Photo)
            .Include(x => x.MapPins)
            .Include(x => x.Notes)
            .FirstOrDefaultAsync();

        if (campaign == null)
        {
            return Result<string>.Failure("Failed to find campaign with id or you are not the DM: " + id, 400);
        }

        if (campaign.Photo != null)
        {
            await cloudinaryService.DeletePhoto(campaign.Photo.PublicId);
        }

        context.MapPins.RemoveRange(campaign.MapPins);
        context.Notes.RemoveRange(campaign.Notes);
        context.Campaigns.Remove(campaign);

        var result = await context.SaveChangesAsync() > 0;

        return !result
            ? Result<string>.Failure("Failed to delete the campaign", 400)
            : Result<string>.Success($"Campaign with id: {id} deleted successfully");
    }

    public async Task<Result<string>> CreateCampaign(CreateCampaignDto campaign)
    {
        var user = await userAccessor.GetUserAsync();

        var newCampaign = mapper.Map<Campaign>(campaign);
        newCampaign.DungeonMaster = user;

        await context.Campaigns.AddAsync(newCampaign);

        var result = await context.SaveChangesAsync() > 0;

        var notes = await notesService.CreateCampaignNotesForUser(user, newCampaign);

        if (!notes.IsSuccess)
        {
            return Result<string>.Failure("Failed to create notes for player to the campaign", 400);
        }


        return !result
            ? Result<string>.Failure("Failed to create the campaign", 400)
            : Result<string>.Success($"New campaign Id: {newCampaign.Id}, DM: {newCampaign.DungeonMaster.DisplayName}");
    }


    public async Task<Result<string>> SetCampaignMapPins(string campaignId, List<MapPinDto> pins)
    {
        var user = await userAccessor.GetUserAsync();

        var campaign = await context.Campaigns
            .Where(x => x.Id == campaignId && x.DungeonMaster.Id == user.Id)
            .FirstOrDefaultAsync();

        if (campaign == null)
        {
            return Result<string>.Failure("Failed to find campaign with id or you are not the DM: " + campaignId,
                400);
        }

        var existingPins = await context.MapPins
            .Where(x => x.CampaignId == campaignId)
            .ToListAsync();

        foreach (var pin in pins)
        {
            var existingPin = existingPins.FirstOrDefault(x => x.Id == pin.Id);
            if (existingPin != null)
            {
                existingPin.Title = pin.Title;
                existingPin.Description = new HtmlSanitizer().Sanitize(pin.Description);;
                existingPin.PositionX = pin.PositionX;
                existingPin.PositionY = pin.PositionY;
                existingPin.Icon = pin.Icon;
            }
            else
            {
                var newPin = mapper.Map<MapPin>(pin);
                newPin.CampaignId = campaignId;
                await context.MapPins.AddAsync(newPin);
            }
        }

        foreach (var pin in existingPins)
        {
            if (pins.All(x => x.Id != pin.Id))
            {
                context.MapPins.Remove(pin);
            }
        }

        var result = await context.SaveChangesAsync() > 0;


        return !result
            ? Result<string>.Failure("Failed to save map pins", 400)
            : Result<string>.Success("Map pins saved successfully");
    }

    public async Task<Result<string>> AddPlayerToCampaign(string campaignId, AddPlayerToCampaignDto player)
    {
        var user = await userAccessor.GetUserAsync();

        var campaign = await context.Campaigns
            .Where(x => x.Id == campaignId && x.DungeonMaster.Id == user.Id)
            .Include(campaign => campaign.Users)
            .Include(x => x.DungeonMaster)
            .FirstOrDefaultAsync();

        if (campaign == null)
        {
            return Result<string>.Failure("Failed to find campaign with id or you are not the DM: " + campaignId,
                400);
        }


        var foundPlayer = await context.Users
            .Where(x => x.UserName == player.Username)
            .FirstOrDefaultAsync();

        if (foundPlayer == null)
        {
            return Result<string>.Failure("Failed to find player with e-mail: " + player.Username, 400);
        }

        if (foundPlayer.UserName == campaign.DungeonMaster.UserName)
        {
            return Result<string>.Failure("Dungeon master cannot participate as a player", 400);
        }

        if (campaign.Users.Any(x => x.Id == foundPlayer.Id))
        {
            return Result<string>.Failure("Player is already in the campaign", 400);
        }

        var notes = await notesService.CreateCampaignNotesForUser(foundPlayer, campaign);

        if (!notes.IsSuccess)
        {
            return Result<string>.Failure("Failed to create notes for player to the campaign", 400);
        }

        campaign.Users.Add(foundPlayer);

        var result = await context.SaveChangesAsync() > 0;

        return !result
            ? Result<string>.Failure("Failed to add player to the campaign", 400)
            : Result<string>.Success($"Player with id: {player} added to campaign with id: {campaignId}");
    }
}
