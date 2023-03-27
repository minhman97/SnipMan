using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories;

public interface IUnitOfWork
{
    IRepository<Snippet> SnippetRepository { get; }
    IRepository<User> UserRepository { get; }
    IRepository<SnippetTag> SnippetTagRepository { get; }
    IRepository<Tag> TagRepository { get; }
    SnippetDto Map(Snippet snippet);
    Task SaveChangesAsync(CancellationToken ct = default);
}