using Hovedopgave.Core.Data;
using Hovedopgave.Core.Interfaces;
using Hovedopgave.Core.Results;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Photos.Models;
using Microsoft.EntityFrameworkCore;

namespace Hovedopgave.Features.Photos.Services;

public class PhotoService(ICloudinaryService cloudinaryService, IUserAccessor userAccessor, AppDbContext context)
    : IPhotoService
{
    public async Task<Result<Photo>> AddCampaignPhoto(IFormFile file, string campaignId)
    {
        var campaign = await context.Campaigns
            .Include(c => c.DungeonMaster)
            .Include(campaign => campaign.Photo)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign == null)
        {
            return Result<Photo>.Failure("Campaign not found", 404);
        }

        var user = await userAccessor.GetUserAsync();

        if (campaign.DungeonMaster.Id != user.Id)
        {
            return Result<Photo>.Failure("You are not the DM of this campaign", 403);
        }


        var photoResult = await UploadAndPreparePhotoEntity(file, user.Id);
        if (!photoResult.IsSuccess)
        {
            return photoResult;
        }

        var newPhoto = photoResult.Value;

        if (newPhoto == null)
        {
            return Result<Photo>.Failure("Photo processing failed: received null photo data despite reported success", 500);
        }

        context.Photos.Add(newPhoto);

        campaign.Photo = newPhoto;

        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<Photo>.Success(newPhoto)
            : Result<Photo>.Failure("Problem saving photo to DB", 400);
    }

    public async Task<Result<Photo>> AddWikiEntryPhoto(IFormFile file, string campaignId)
    {
        var campaign = await context.Campaigns
          .Include(c => c.DungeonMaster)
          .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign == null)
        {
            return Result<Photo>.Failure("Campaign not found", 404);
        }

        var user = await userAccessor.GetUserAsync();

        if (campaign.DungeonMaster.Id != user.Id)
        {
            return Result<Photo>.Failure("You are not the DM of this campaign", 403);
        }


        var photoResult = await UploadAndPreparePhotoEntity(file, user.Id);
        if (!photoResult.IsSuccess)
        {
            return photoResult;
        }

        var newPhoto = photoResult.Value;

        if (newPhoto == null)
        {
            return Result<Photo>.Failure("Photo processing failed: received null photo data despite reported success", 500);
        }

        context.Photos.Add(newPhoto);

        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<Photo>.Success(newPhoto)
            : Result<Photo>.Failure("Problem saving photo to DB", 400);
    }

    public async Task<Result<Photo>> AddCharacterPhoto(IFormFile file, string characterId)
    {
        var user = await userAccessor.GetUserAsync();

        var character = await context.Characters.FindAsync(characterId);

        if (character == null)
        {
            return Result<Photo>.Failure("Character not found", 404);
        }

        if (character.UserId != user.Id)
        {
            return Result<Photo>.Failure("You are not the owner of the character", 400);
        }

        var photoResult = await UploadAndPreparePhotoEntity(file, user.Id);
        if (!photoResult.IsSuccess)
        {
            return photoResult;
        }

        var newPhoto = photoResult.Value;

        if (newPhoto == null)
        {
            return Result<Photo>.Failure("Photo processing failed: received null photo data despite reported success", 500);
        }

        context.Photos.Add(newPhoto);

        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<Photo>.Success(newPhoto)
            : Result<Photo>.Failure("Problem saving photo to DB", 400);

    }

    private async Task<Result<Photo>> UploadAndPreparePhotoEntity(IFormFile file, string userId)
    {
        var uploadResult = await cloudinaryService.UploadPhoto(file);

        if (uploadResult == null)
        {
            return Result<Photo>.Failure("Failed to upload photo to image service", 400);
        }

        var photo = new Photo
        {
            Url = uploadResult.Url,
            PublicId = uploadResult.PublicId,
            UserId = userId
        };

        return Result<Photo>.Success(photo);
    }
}
