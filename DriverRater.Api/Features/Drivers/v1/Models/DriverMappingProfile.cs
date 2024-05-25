namespace DriverRater.Api.Features.Drivers.v1.Models;

using DriverRater.Api.Entities;
using DriverRater.Api.Features.Drivers.v1.Commands;
using DriverRater.Api.Features.Drivers.v1.Queries;
using DriverRater.Shared;
using DriverRater.Shared.Drivers.v1.Models;
using Mapster;


public class DriverMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
       config.NewConfig<Guid, GetDriversForUser.Query>()
            .Map(dest => dest.UserContext.ProfileId, src => src);

        config.NewConfig<RankedDriver, DriversRankModel>()
            .Map(drm => drm.LastRankedDate, src => src.DateRanked)
            .Map(model => model.DriverRank, src => src.Rank)
            .Map(model => model.UserId, src => src.RankedBy.Id);

        config.NewConfig<UpdateDriverRankRequest, UpdateDriverRank.Command>()
            .Map(dest => dest.NewRank, src => src.NewRank.ConvertToDriverRank());

        config.NewConfig<IUserContext, UpdateDriverRank.Command>()
            .Map(dest => dest.Profile, src => src);

        config.NewConfig<UpdateDriverRank.Response, UpdateDriverRankResponse>()
            .Map(dest => dest.Rank, src => src.Rank.ConvertToRank());
    }
}