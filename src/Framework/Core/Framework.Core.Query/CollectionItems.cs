namespace Framework.Core.Query;

public class CollectionItems<T>
{
    public CollectionItems(IEnumerable<T> items, long totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    public IEnumerable<T> Items { get; set; }
    public long TotalCount { get; set; }
}