using Hovedopgave.Core.Controllers;
using Hovedopgave.Features.Photos.Models;
using Hovedopgave.Features.Photos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hovedopgave.Features.Photos;

public class PhotosController(IPhotoService photoService) : BaseApiController
{
    [HttpPost("add-campaign-photo/{campaignId}")]
    public async Task<ActionResult<Photo>> AddCampaignPhoto(IFormFile file, string campaignId)
    {
        var result = await photoService.AddCampaignPhoto(file, campaignId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("add-wiki-entry-photo/{campaignId}")]
    public async Task<ActionResult<Photo>> AddWikiEntryPhoto(IFormFile file, string campaignId)
    {
        var result = await photoService.AddWikiEntryPhoto(file, campaignId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("add-character-photo/{characterId}")]
    public async Task<ActionResult<Photo>> AddCharacterPhoto(IFormFile file, string characterId)
    {
        var result = await photoService.AddCharacterPhoto(file, characterId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
