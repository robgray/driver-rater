namespace DriverRater.Api.Features.RacingService.v1;

using AutoMapper;
using DriverRater.Api.Features.RacingService.v1.Queries;
using DriverRater.Api.Features.Shared;
using DriverRater.Api.Services;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[ApiController]
public class RacingServiceController : BaseController
{
    private IUserContext userContext;
    
    public RacingServiceController(IMediator mediator, IMapper mapper, IUserContext userContext) : base(mediator, mapper)
    {
        this.userContext = userContext;
    }

    [Authorize]
    [HttpGet("{customerId:int}/recent")]
    [ProducesResponseType(typeof(IEnumerable<RecentMemberRaceSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Recent(int customerId) =>
        await ExecuteQuery<GetMemberRecentRaces.Query, IEnumerable<RecentMemberRaceSummary>>(customerId);

    [Authorize]
    [HttpGet("{subsessionId:int}/drivers/{userId:Guid}")]
    public async Task<IActionResult> Drivers(int subsessionId, Guid userId) =>
        await ExecuteQuery<GetRacersInSubsession.Query, IEnumerable<DriversRankModel>>(subsessionId, userContext.ProfileId);

}
