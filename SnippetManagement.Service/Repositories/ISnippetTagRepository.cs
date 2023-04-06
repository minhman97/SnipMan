using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetTagRepository : IRepository<SnippetTag>
{
    Task<IEnumerable<SnippetTag>> GetSnippetTagsBySnippetId(Guid id);
}