using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetTagRepository : IRepository<SnippetTag>
{
    Task<IEnumerable<SnippetTag>> GetSnippetTagsBySnippetId(Guid id);
}