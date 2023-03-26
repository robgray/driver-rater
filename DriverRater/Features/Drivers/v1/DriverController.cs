namespace DriverRater.Features.Drivers.v1;

using AutoMapper;
using DriverRater.Features.Drivers.v1.Commands;
using DriverRater.Features.Drivers.v1.Models;
using DriverRater.Features.Drivers.v1.Queries;
using DriverRater.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class DriverController : BaseController
{
    public DriverController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(IEnumerable<DriversRankModel>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(Guid userId) =>
        ExecuteQuery<GetDriversForUser.Query, IEnumerable<DriversRankModel>>(userId);

    [HttpPost("rank")]
    [ProducesResponseType(typeof(UpdateDriverRankResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> Post([FromBody] UpdateDriverRankRequest request) =>
        ExecuteCommand<UpdateDriverRank.Command, UpdateDriverRankResponse>(request);
}