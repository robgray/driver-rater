namespace DriverRater.Features.HelmetPack.v1;

using AutoMapper;
using DriverRater.Features.HelmetPack.v1.Queries;
using DriverRater.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class HelmetPackController : BaseController
{
    public HelmetPackController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("{userId:Guid}")]
    public async Task<IActionResult> DownloadHelmetPackForUser(Guid userId)
    {
        // this is going to return a zip file with all helmets I've ranked.
        var response = await ExecuteMediatorRequest<BuildHelmetPack.Command, BuildHelmetPack.Response>(userId);

        return File(response.ZilFileData, "application/zip", response.Filename);
    }

    [HttpGet("all")]
    public Task<IActionResult> DownloadHelmetPackForAll()
    {
        // Downloads a helmet pack based on the average of all users inputs.
        throw new NotImplementedException();
    }
}