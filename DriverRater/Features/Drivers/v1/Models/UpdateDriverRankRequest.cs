namespace HelmetRanker.Features.Drivers.v1.Models;

using System.ComponentModel.DataAnnotations;

public class UpdateDriverRankRequest
{
    [Range(0, int.MaxValue)]
    public int NewRank { get; set; }
}