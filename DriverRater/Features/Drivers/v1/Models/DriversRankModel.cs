namespace HelmetRanker.Features.Drivers.v1.Models;

using HelmetRanker.Entities;

public record DriversRankModel(Guid Id, string Name, int RacingId, DriverRank DriverRank, Guid UserId);