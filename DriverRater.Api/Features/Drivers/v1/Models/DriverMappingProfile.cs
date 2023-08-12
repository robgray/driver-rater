namespace DriverRater.Api.Features.Drivers.v1.Models;

using DriverRater.Api.Entities;
using DriverRater.Api.Features.Drivers.v1.Commands;
using DriverRater.Api.Features.Drivers.v1.Queries;
using DriverRater.Api.Services;
using DriverRater.Shared.Drivers.v1.Models;
using Profile = AutoMapper.Profile;

public class DriverMappingProfile : Profile
{
    public DriverMappingProfile()
    {
        CreateMap<Guid, GetDriversForUser.Query>()
            .ForMember(q => q.UserId, o => o.MapFrom(g => g));
        CreateMap<RankedDriver, DriversRankModel>()
            .ForMember(drm => drm.LastRankedDate, o => o.MapFrom(d => d.DateRanked))
            .ForMember(drm => drm.DriverRank, o => o.MapFrom(d => d.Rank))
            .ForPath(drm => drm.UserId, o => o.MapFrom(d => d.RankedBy.Id));

        CreateMap<UpdateDriverRankRequest, UpdateDriverRank.Command>()
            .ForMember(d => d.NewRank, o => o.MapFrom(s => s.NewRank.ConvertToDriverRank()));

        CreateMap<IUserContext, UpdateDriverRank.Command>()
            .ForMember(d => d.Profile, o => o.MapFrom(profile => profile));

        CreateMap<UpdateDriverRank.Response, UpdateDriverRankResponse>()
            .ForMember(d => d.Rank, o => o.MapFrom(d => d.Rank.ConvertToRank()));
    }   
}