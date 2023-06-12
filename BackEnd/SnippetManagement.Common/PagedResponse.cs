using SnippetManagement.Common.Enum;

namespace SnippetManagement.Common;

public class PagedResponse<T>
{
    public T Data { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}

public class Pagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public Pagination()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    public Pagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 10 ? 10 : pageSize;
    }
}
public class RangeDataResponse<T>
{
    public T? Data { get; set; }
    public int StartIndex { get; set; } = 0;
    public int EndIndex { get; set; } = 6;
    public int TotalRecords { get; set; }
}

public class SortOrder
{
    public OrderWay OrderWay { get; set; } = OrderWay.Asc;
    public string? Property { get; set; }
}