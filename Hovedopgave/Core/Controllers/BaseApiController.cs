using Hovedopgave.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace Hovedopgave.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        switch (result.IsSuccess)
        {
            case false when result.Code == 404:
                return NotFound();
            case true when result.Value != null:
                return Ok(result.Value);
            default:
                return BadRequest(result.Error);
        }
    }
}