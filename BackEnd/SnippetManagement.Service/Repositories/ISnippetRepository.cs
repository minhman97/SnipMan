using SnippetManagement.Common;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetRepository : IRepository<Snippet>
{
    SnippetDto? Map(Snippet snippet);
    Task<PagedResponse<IEnumerable<SnippetDto>>> Search(SearchSnippetRequest request);
    Task<PagedResponse<IEnumerable<SnippetDto?>>> GetAll(Pagination pagination);
    Task<RangeDataResponse<IEnumerable<SnippetDto>>> GetRange(Guid userId, int startIndex, int endIndex, SortOrder sortOrder);

    Task<RangeDataResponse<IEnumerable<SnippetDto>>> SearchRange(Guid userId, int startIndex, int endIndex,
        SearchSnippetRequest request, SortOrder sortOrder);

    Task<SnippetDto> GetShareableSnippet(Guid userId, Guid shareableId);
}