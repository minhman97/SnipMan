using SnippetManagement.Common;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetRepository : IRepository<Snippet>
{
    Task<Snippet?> Find(Guid id);
    SnippetDto? Map(Snippet snippet);
    Task<PagedResponse<IEnumerable<SnippetDto>>> Search(SearchSnippetRequest request);
    Task<PagedResponse<IEnumerable<SnippetDto?>>> GetAll(Pagination pagination);
    Task<PagedRangeResponse<IEnumerable<SnippetDto>>> GetRange(int startIndex, int endIndex, SortOrder sortOrder);

    Task<PagedRangeResponse<IEnumerable<SnippetDto>>> SearchRange(int startIndex, int endIndex,
        SearchSnippetRequest request, SortOrder sortOrder);
}