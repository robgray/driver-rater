namespace HelmetRanker.Plumbing.Mediator;

public class PagedResults<T>
{
    public PagedResults() { }

    public PagedResults(T[] items, int currentPage, int totalPages, int totalItems)
    {
        Items = items;
        CurrentPage = currentPage;
        TotalPages = totalPages;
        TotalItems = totalItems;
    }

    public T[] Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}
