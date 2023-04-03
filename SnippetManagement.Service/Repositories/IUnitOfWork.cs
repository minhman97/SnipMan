using SnippetManagement.Data;

namespace SnippetManagement.Service.Repositories;

public interface IUnitOfWork
{
    SnippetManagementDbContext Context { get; }
    ISnippetRepository SnippetRepository { get; }
    IUserRepository UserRepository { get; }
    ISnippetTagRepository SnippetTagRepository { get; }
    ITagRepository TagRepository { get; }
    Task SaveChangesAsync(CancellationToken ct = default);
}