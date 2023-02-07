using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Implementation;

public class SnippetService : ISnippetService
{
    private SnippetManagementDbContext _context;
    private readonly ISnippetTagService _snippetTagService;

    public SnippetService(SnippetManagementDbContext context, ISnippetTagService snippetTagService)
    {
        _context = context;
        _snippetTagService = snippetTagService;
    }

    public async Task<SnippetDto> Create(CreateSnippetRequest request)
    {
        var snippet = new Snippet()
        {
            Content = request.Content,
            Name = request.Name,
            Description = request.Description,
            Origin = request.Origin
        };
        
        await _context.AddAsync(snippet);
        await _context.SaveChangesAsync();
        
        return Map(snippet);
    }

    public async Task<SnippetDto> Get(Guid id)
    {
        return Map(await _context.Set<DataModel.Snippet>().FindAsync(id));
    }

    public async Task<SnippetDto> Update(UpdateSnippetRequest request)
    {
        var snippet = await _context.Set<DataModel.Snippet>().FindAsync(request.Id);
        if (snippet is null)
            return null;
        snippet.Content = request.Content;
        snippet.Description = request.Description;
        snippet.Name = snippet.Name;
        snippet.Modified = DateTimeOffset.UtcNow;

        _context.Update(snippet);
        await _context.SaveChangesAsync();
        return Map(snippet);
    }

    public async Task Delete(Guid id)
    {
        var snippet = await _context.Set<DataModel.Snippet>().FindAsync(id);
        if (snippet is not null)
        {
            _context.Remove(snippet);
            await _context.SaveChangesAsync();
        }
    }

    private SnippetDto Map(Snippet snippet)
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
        var list = new List<TagDto>();
        foreach (var tag in tags)
        {
            list.Add(new TagDto()
            {
                Id = tag.TagId,
                TagName = tag.Tag.TagName,
            });
        }
        return list;
    }
}