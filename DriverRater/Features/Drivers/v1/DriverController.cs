namespace HelmetRanker.Features.Drivers.v1;

using AutoMapper;
using HelmetRanker.Features.Drivers.v1.Commands;
using HelmetRanker.Features.Drivers.v1.Models;
using HelmetRanker.Features.Drivers.v1.Queries;
using HelmetRanker.Features.Shared;
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> Post([FromBody] NewDriverRankRequest request) =>
        ExecuteCommand<NewDriverRank.Command>(request);

    [HttpPut("{driverId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> Put(Guid driverId, UpdateDriverRankRequest request) =>
        ExecuteCommand<UpdateDriverRank.Command>(driverId, request);
}