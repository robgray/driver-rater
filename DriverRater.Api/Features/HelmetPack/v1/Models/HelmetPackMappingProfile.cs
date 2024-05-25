namespace DriverRater.Api.Features.HelmetPack.v1.Models;

using AutoMapper;
using DriverRater.Api.Features.HelmetPack.v1.Commands;

public class HelmetPackMappingProfile : Profile
{
    public HelmetPackMappingProfile()
    {
        CreateMap<Guid, BuildHelmetPack.Command>()
            .ForMember(c => c.UserContext, o => o.MapFrom(uc => uc));
    }
}