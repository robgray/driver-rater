namespace DriverRater.Api.Features.Drivers.v1;

using AutoMapper;
using DriverRater.Api.Features.Drivers.v1.Commands;
using DriverRater.Api.Features.Drivers.v1.Queries;
using DriverRater.Api.Features.Shared;
using DriverRater.Api.Services;
using DriverRater.Shared.Drivers.v1.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class DriverController : BaseController
{
    private IUserContext userContext;

    public DriverController(IMediator mediator, IMapper mapper, IUserContext userContext) 
        : base(mediator, mapper)
    {
        this.userContext = userContext;
    }

    [Authorize]
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(IEnumerable<DriversRankModel>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(Guid userId) =>
        ExecuteQuery<GetDriversForUser.Query, IEnumerable<DriversRankModel>>(userId);

    [Authorize]
    [HttpPost("rank")]
    [ProducesResponseType(typeof(UpdateDriverRankResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> Post([FromBody] UpdateDriverRankRequest request) =>
        ExecuteCommand<UpdateDriverRank.Command, UpdateDriverRankResponse>(request, userContext);
}