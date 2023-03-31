namespace DriverRater.Features.HelmetPack.v1.Models;

using AutoMapper;
using DriverRater.Features.HelmetPack.v1.Queries;

public class HelmetPackMappingProfile : Profile
{
    public HelmetPackMappingProfile()
    {
        CreateMap<Guid, BuildHelmetPack.Command>()
            .ForMember(c => c.UserId, o => o.MapFrom(g => g));
    }
}