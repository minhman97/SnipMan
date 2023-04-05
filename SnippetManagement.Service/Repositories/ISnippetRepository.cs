using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetRepository : IRepository<Snippet>
{
    Task<Snippet?> Find(Guid id);
    SnippetDto? Map(Snippet snippet);
    Task<IEnumerable<SnippetDto?>> GetAll();
    Task<List<SnippetDto>> Search(FilterSnippetRequest request);
}