using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetRepository: IRepository<Snippet>
{
    Task<Snippet?> Find(Guid id);
    void Remove(Snippet snippet);
}