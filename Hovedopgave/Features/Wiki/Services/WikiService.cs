using AutoMapper;
using Ganss.Xss;
using Hovedopgave.Core.Data;
using Hovedopgave.Core.Interfaces;
using Hovedopgave.Core.Results;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Wiki.DTOs;
using Hovedopgave.Features.Wiki.Models;
using Microsoft.EntityFrameworkCore;

namespace Hovedopgave.Features.Wiki.Services;

public class WikiService(
    AppDbContext context,
    IUserAccessor userAccessor,
    IMapper mapper,
    ICloudinaryService cloudinaryService) : IWikiService
{
    public async Task<Result<string>> CreateWikiEntry(CreateWikiEntryDto wikiEntryDto)
    {
        var user = await userAccessor.GetUserAsync();

        var campaign = await context.Campaigns
            .Where(x => x.Id == wikiEntryDto.CampaignId && x.DungeonMaster.Id == user.Id)
            .Include(x => x.Users)
            .FirstOrDefaultAsync();

        if (campaign == null)
        {
            return Result<string>.Failure("Failed to find campaign with id or you are not the DM", 400);
        }

        var entry = mapper.Map<WikiEntry>(wikiEntryDto);

        var sanitizer = new HtmlSanitizer();

        entry.Content = sanitizer.Sanitize(wikiEntryDto.Content);
        entry.Campaign = campaign;

        if (!string.IsNullOrEmpty(wikiEntryDto.PhotoId))
        {
            var photo = await context.Photos
                .Where(x => x.Id == wikiEntryDto.PhotoId)
                .FirstOrDefaultAsync();
            if (photo != null)
            {
                entry.Photo = photo;
            }
        }

        await context.WikiEntries.AddAsync(entry);

        var result = await context.SaveChangesAsync() > 0;

        return !result
            ? Result<string>.Failure("Failed to create wiki entry", 400)
            : Result<string>.Success(entry.Id);
    }

    public async Task<Result<List<WikiEntryDto>>> GetWikiEntriesForCampaign(string campaignId)
    {
        var campaign = await context.Campaigns
            .Where(x => x.Id == campaignId)
            .FirstOrDefaultAsync();

        if (campaign == null)
        {
            return Result<List<WikiEntryDto>>.Failure($"Failed to find campaign with id: {campaignId}", 404);
        }

        var wikiEntries = await context.WikiEntries
            .Where(x => x.CampaignId == campaignId)
            .Include(x => x.Photo)
            .ToListAsync();

        return Result<List<WikiEntryDto>>.Success(mapper.Map<List<WikiEntryDto>>(wikiEntries));
    }

    public async Task<Result<string>> DeleteWikiEntry(string wikiEntryId)
    {
        var user = await userAccessor.GetUserAsync();

        var entry = await context.WikiEntries
            .Include(x => x.Photo)
            .Include(x => x.Campaign.DungeonMaster)
            .Where(x => x.Id == wikiEntryId)
            .FirstOrDefaultAsync();

        if (entry == null)
        {
            return Result<string>.Failure("Failed to find wiki entry", 404);
        }

        if (entry.Campaign.DungeonMaster.Id != user.Id)
        {
            return Result<string>.Failure("Only the DM can delete wiki entries", 403);
        }

        if (entry.Photo != null)
        {
            await cloudinaryService.DeletePhoto(entry.Photo.PublicId);
        }

        context.WikiEntries.Remove(entry);

        var result = await context.SaveChangesAsync() > 0;

        return !result
            ? Result<string>.Failure("Failed to delete wiki entry", 400)
            : Result<string>.Success(entry.Id);
    }

    public async Task<Result<WikiEntryDto>> UpdateWikiEntry(WikiEntryDto wikiEntryDto)
    {
        var wikiEntry = await context.WikiEntries.FindAsync(wikiEntryDto.Id);

        if (wikiEntry is null)
        {
            return Result<WikiEntryDto>.Failure("Failed to find wiki entry", 404);
        }

        if (wikiEntryDto.Xmin != wikiEntry.Xmin)
        {
            return Result<WikiEntryDto>.Failure("Entry has already been updated recently", 400);
        }

        var sanitizer = new HtmlSanitizer();

        wikiEntry.Name = wikiEntryDto.Name;
        wikiEntry.Content = sanitizer.Sanitize(wikiEntryDto.Content);
        wikiEntry.Type = wikiEntryDto.Type;
        wikiEntry.IsVisible = wikiEntryDto.IsVisible;

        context.WikiEntries.Update(wikiEntry);

        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<WikiEntryDto>.Success(mapper.Map<WikiEntryDto>(wikiEntry))
            : Result<WikiEntryDto>.Failure("Failed to update wiki entry", 400);
    }

    public async Task<Result<WikiEntryDto>> GetWikiEntry(string entryId)
    {
        var wikiEntry = await context.WikiEntries
            .Where(x => x.Id == entryId)
            .Include(x => x.Photo)
            .FirstAsync();

        if (wikiEntry is null)
        {
            return Result<WikiEntryDto>.Failure("Failed to find wiki entry", 404);
        }

        return Result<WikiEntryDto>.Success(mapper.Map<WikiEntryDto>(wikiEntry));
    }
}
