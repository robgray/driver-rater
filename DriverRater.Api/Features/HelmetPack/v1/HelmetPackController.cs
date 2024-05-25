namespace DriverRater.Api.Features.HelmetPack.v1;

using AutoMapper;
using DriverRater.Api.Features.HelmetPack.v1.Commands;
using DriverRater.Api.Features.Shared;
using DriverRater.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class HelmetPackController(IMediator mediator, IMapper mapper, IUserContext userContext) 
    : BaseController(mediator, mapper)
{
    [Authorize]
    [HttpGet("{userId:Guid}")]
    public async Task<IActionResult> DownloadHelmetPackForUser()
    {
        // this is going to return a zip file with all helmets I've ranked.
        var response = await ExecuteMediatorRequest<BuildHelmetPack.Command, BuildHelmetPack.Response>(userContext);

        return File(response.ZilFileData, "application/zip", response.Filename);
    }

    [AllowAnonymous]
    [HttpGet("all")]
    public Task<IActionResult> DownloadHelmetPackForAll()
    {
        // Downloads a helmet pack based on the average of all users inputs.
        throw new NotImplementedException();
    }
}