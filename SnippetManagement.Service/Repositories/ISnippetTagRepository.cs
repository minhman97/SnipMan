using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories;

public interface ISnippetTagRepository
{
    Task<IEnumerable<SnippetTag>> GetSnippetTagsBySnippetId(Guid id);
}