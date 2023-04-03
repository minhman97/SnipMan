using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetRepository: IRepository<Snippet>
{
    Task<Snippet?> Find(Guid id);
    SnippetDto Map(Snippet snippet);
}