namespace DriverRater.Api.Features.RacingService.v1;

using AutoMapper;
using DriverRater.Api.Features.RacingService.v1.Queries;
using DriverRater.Api.Features.Shared;
using DriverRater.Api.Plumbing.Startup.Auth;
using DriverRater.Shared;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[ApiController]
public class RacingServiceController(IMediator mediator, IMapper mapper, IUserContext userContext) 
    : BaseController(mediator, mapper)
{
    [Authorize(Policy = AuthPolicies.Racer)]
    [HttpGet("{customerId:int}/recent")]
    [ProducesResponseType(typeof(IEnumerable<RecentMemberRaceSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Recent(int customerId) =>
        await ExecuteQuery<GetMemberRecentRaces.Query, IEnumerable<RecentMemberRaceSummary>>(customerId);

    [Authorize(Policy = AuthPolicies.Racer)]
    [HttpGet("{subsessionId:int}/drivers")]
    public async Task<IActionResult> Drivers(int subsessionId) =>
        await ExecuteQuery<GetRacersInSubsession.Query, IEnumerable<DriversRankModel>>(subsessionId, userContext.ProfileId);

}
