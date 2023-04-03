using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories.Implementation;
public class UnitOfWork : IUnitOfWork
{
    public SnippetManagementDbContext Context { get; }
    private readonly TagRepository _tagRepository;
    private readonly SnippetRepository _snippetRepository;
    private readonly UserRepository _userRepository;
    private readonly SnippetTagRepository _snippetTagRepository;


    public UnitOfWork(SnippetManagementDbContext context, TagRepository tagRepository,  SnippetRepository snippetRepository, UserRepository userRepository, SnippetTagRepository snippetTagRepository)
    {
        Context = context;
        _tagRepository = tagRepository;
        _snippetRepository = snippetRepository;
        _userRepository = userRepository;
        _snippetTagRepository = snippetTagRepository;
    }
    
    public ITagRepository TagRepository => _tagRepository;

    public ISnippetRepository SnippetRepository => _snippetRepository;

    public IUserRepository UserRepository => _userRepository;

    public ISnippetTagRepository SnippetTagRepository => _snippetTagRepository;

    public Task SaveChangesAsync(CancellationToken ct = default)
        => Context.SaveChangesAsync(ct);
    
    
}