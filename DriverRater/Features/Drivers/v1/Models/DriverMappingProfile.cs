namespace DriverRater.Features.Drivers.v1.Models;

using AutoMapper;
using DriverRater.Entities;
using DriverRater.Features.Drivers.v1.Commands;
using DriverRater.Features.Drivers.v1.Queries;

public class DriverMappingProfile : Profile
{
    public DriverMappingProfile()
    {
        CreateMap<Guid, GetDriversForUser.Query>()
            .ForMember(q => q.UserId, o => o.MapFrom(g => g));
        CreateMap<RankedDriver, DriversRankModel>()
            .ForMember(drm => drm.DriverRank, o => o.MapFrom(d => d.Rank))
            .ForPath(drm => drm.UserId, o => o.MapFrom(d => d.RankedBy.Id));

        CreateMap<UpdateDriverRankRequest, UpdateDriverRank.Command>()
            .ForMember(d => d.NewRank, o => o.MapFrom(s => (DriverRank)s.NewRank));

        CreateMap<UpdateDriverRank.Response, UpdateDriverRankResponse>()
            .ForMember(d => d.Rank, o => o.MapFrom(d => (int)d.Rank));
    }   
}