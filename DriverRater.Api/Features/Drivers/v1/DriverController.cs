namespace DriverRater.Api.Features.Drivers.v1;

using AutoMapper;
using DriverRater.Api.Features.Drivers.v1.Commands;
using DriverRater.Api.Features.Drivers.v1.Queries;
using DriverRater.Api.Features.Shared;
using DriverRater.Shared;
using DriverRater.Shared.Drivers.v1.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class DriverController(IMediator mediator, IMapper mapper, IUserContext userContext)
    : BaseController(mediator, mapper)
{
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DriversRankModel>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get() =>
        ExecuteQuery<GetDriversForUser.Query, IEnumerable<DriversRankModel>>(userContext);

    [Authorize]
    [HttpPost("rank")]
    [ProducesResponseType(typeof(UpdateDriverRankResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> Post([FromBody] UpdateDriverRankRequest request) =>
        ExecuteCommand<UpdateDriverRank.Command, UpdateDriverRankResponse>(request, userContext);
}