using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories.Implementation;
public class UnitOfWork : IUnitOfWork
{
    private readonly SnippetManagementDbContext _context;
    private readonly IRepository<Snippet> _snippetRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<SnippetTag> _snippetTagRepository;
    private readonly IRepository<User> _userRepository;



    public UnitOfWork(SnippetManagementDbContext context, IRepository<Snippet> snippetRepository, IRepository<Tag> tagRepository, IRepository<SnippetTag> snippetTagRepository, IRepository<User> userRepository)
    {
        _context = context;
        _snippetRepository = snippetRepository;
        _tagRepository = tagRepository;
        _snippetTagRepository = snippetTagRepository;
        _userRepository = userRepository;
    }
    
    public IRepository<Tag> TagRepository
    {
        get
        {
            return _tagRepository;
            // if (_tagRepository is null)
            //     _tagRepository = new TagRepository(_context);
            // return _tagRepository;
        }
    }

    public IRepository<Snippet> SnippetRepository
    {
        get
        {
            return _snippetRepository;

            // if (_snippetRepository is null)
            //     _snippetRepository = new SnippetRepository(_context);
            // return _snippetRepository;
        }
    }

    public IRepository<User> UserRepository
    {
        get
        {
            return _userRepository;
            // if(_userRepository is null)
            //     _userRepository = new UserRepository(_context);
            // return _userRepository;
        }
    }
    
    public IRepository<SnippetTag> SnippetTagRepository
    {
        get
        {
            return _snippetTagRepository;
            // if(_snippetTagRepository is null)
            //     _snippetTagRepository =new SnippetTagRepository(_context);
            // return _snippetTagRepository;
        }
    }
    
    public Task SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
    
    public SnippetDto Map(Snippet snippet)
    {
        if (snippet is null)
            return null;
        return new SnippetDto()
        {
            Id = snippet.Id,
            Content = snippet.Content,
            Name = snippet.Name,
            Description = snippet.Description,
            Origin = snippet.Origin,
            Created = snippet.Created,
            Modified = snippet.Modified,
            Tags = MapTag(snippet.Tags)
        };
    }

    private IEnumerable<TagDto> MapTag(IEnumerable<SnippetTag> tags)
    {
        return tags.Select(x => new TagDto()
        {
            Id = x.TagId,
            TagName = x.Tag.TagName
        });
    }
}