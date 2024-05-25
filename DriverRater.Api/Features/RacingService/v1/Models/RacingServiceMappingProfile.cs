namespace DriverRater.Api.Features.RacingService.v1.Models;

using AutoMapper;
using DriverRater.Api.Features.RacingService.v1.Queries;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;

public class RacingServiceMappingProfile : Profile
{
    public RacingServiceMappingProfile()
    {
        CreateMap<int, GetMemberRecentRaces.Query>()
            .ForMember(q => q.MemberId, o => o.MapFrom(i => i));
        CreateMap<GetMemberRecentRaces.Response, RecentMemberRaceSummary>();

        CreateMap<int, GetRacersInSubsession.Query>()
            .ForMember(q => q.SubsessionId, o => o.MapFrom(i => i));
        
        CreateMap<Guid, GetRacersInSubsession.Query>()
            .ForMember(q => q.ProfileId, o => o.MapFrom(g => g));
        
        CreateMap<GetRacersInSubsession.Response, DriversRankModel>()
            .ForMember(r => r.Id, o => o.MapFrom(m => m.RankedDriverId))
            .ForMember(r => r.Name, o => o.MapFrom(m => m.DriverName))
            .ForMember(r => r.UserId, o => o.MapFrom(m => m.RankedByUserId)) 
            .ForMember(drm => drm.DriverRank, o => o.MapFrom(d => d.Rank))
            .ForMember(r => r.RacingId, o => o.MapFrom(m => m.MemberId));
    }
}