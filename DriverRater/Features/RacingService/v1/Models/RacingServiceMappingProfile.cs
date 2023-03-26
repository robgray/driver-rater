namespace DriverRater.Features.RaceResults.v1.Models;

using AutoMapper;
using DriverRater.Features.RaceResults.v1.Queries;

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
            .ForMember(q => q.UserId, o => o.MapFrom(g => g));
        CreateMap<GetRacersInSubsession.Response, SubsessionRankedDriver>();
    }
}