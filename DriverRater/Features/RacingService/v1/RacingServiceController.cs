namespace DriverRater.Features.RaceResults.v1;

using AutoMapper;
using DriverRater.Features.RaceResults.v1.Models;
using DriverRater.Features.RaceResults.v1.Queries;
using DriverRater.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[ApiController]
public class RacingServiceController : BaseController
{
    public RacingServiceController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("{customerId}/recent")]
    [ProducesResponseType(typeof(IEnumerable<RecentMemberRaceSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Recent(int customerId) =>
        await ExecuteQuery<GetMemberRecentRaces.Query, IEnumerable<RecentMemberRaceSummary>>(customerId);

    [HttpGet("{subsessionId}/drivers/{userId}")]
    public async Task<IActionResult> Drivers(int subsessionId, Guid userId) =>
        await ExecuteQuery<GetRacersInSubsession.Query, IEnumerable<SubsessionRankedDriver>>(subsessionId, userId);

}