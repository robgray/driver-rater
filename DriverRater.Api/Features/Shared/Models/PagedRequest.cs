namespace DriverRater.Api.Features.Shared.Models;
public class PagedRequest
{
    public const int DefaultPageSize = 10;
    
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = DefaultPageSize;
}