namespace HelmetRanker.Features.Drivers.v1.Models;

using AutoMapper;
using HelmetRanker.Entities;
using HelmetRanker.Features.Drivers.v1.Commands;
using HelmetRanker.Features.Drivers.v1.Queries;
using Microsoft.EntityFrameworkCore;

public class DriverMappingProfile : Profile
{
    public DriverMappingProfile()
    {
        CreateMap<Guid, GetDriversForUser.Query>()
            .ForMember(q => q.UserId, o => o.MapFrom(g => g));
        CreateMap<Driver, DriversRankModel>()
            .ForPath(drm => drm.UserId, o => o.MapFrom(d => d.RankedBy.Id));

        CreateMap<NewDriverRankRequest, NewDriverRank.Command>()
            .ForMember(d => d.Rank, o => o.MapFrom(s => (DriverRank)s.Rank));

        CreateMap<Guid, UpdateDriverRank.Command>()
            .ForMember(c => c.DriverId, o => o.MapFrom(g => g));
        CreateMap<UpdateDriverRankRequest, UpdateDriverRank.Command>()
            .ForMember(c => c.NewRank, o => o.MapFrom(g => g.NewRank));
        
        

    }   
}