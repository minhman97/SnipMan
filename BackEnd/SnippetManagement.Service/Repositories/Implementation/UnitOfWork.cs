using SnippetManagement.Data;

namespace SnippetManagement.Service.Repositories.Implementation;
public class UnitOfWork : IUnitOfWork
{
    public SnippetManagementDbContext Context { get; }


    public UnitOfWork(SnippetManagementDbContext context)
    {
        Context = context;
    }
    
    public ITagRepository TagRepository => new TagRepository(Context);

    public ISnippetRepository SnippetRepository => new SnippetRepository(Context);

    public IUserRepository UserRepository => new UserRepository(Context);

    public ISnippetTagRepository SnippetTagRepository => new SnippetTagRepository(Context);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => Context.SaveChangesAsync(ct);
    
    
}