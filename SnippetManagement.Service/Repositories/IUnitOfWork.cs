using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories;

public interface IUnitOfWork
{
    SnippetManagementDbContext Context { get; }
    ISnippetRepository SnippetRepository { get; }
    IUserRepository UserRepository { get; }
    ISnippetTagRepository SnippetTagRepository { get; }
    ITagRepository TagRepository { get; }
    SnippetDto Map(Snippet snippet);
    Task SaveChangesAsync(CancellationToken ct = default);
}