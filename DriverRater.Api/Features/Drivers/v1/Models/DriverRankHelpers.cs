namespace DriverRater.Api.Features.Drivers.v1.Models;

using DriverRater.Api.Entities;
using DriverRater.Shared;

public static class DriverRankHelpers
{
   public static DriverRank ConvertToDriverRank(this Rank rank)
   {
      return rank switch
      {
         Rank.Blue => DriverRank.Blue,
         Rank.Green => DriverRank.Green,
         Rank.Yellow => DriverRank.Yellow,
         Rank.Red => DriverRank.Red,
         Rank.Black => DriverRank.Black,
         _ => DriverRank.None
      };
   }
   
   public static Rank ConvertToRank(this DriverRank rank)
   {
      return rank switch
      {
         DriverRank.Blue => Rank.Blue,
         DriverRank.Green => Rank.Green,
         DriverRank.Yellow => Rank.Yellow,
         DriverRank.Red => Rank.Red,
         DriverRank.Black => Rank.Black,
         _ => Rank.None,
      };
   }
}