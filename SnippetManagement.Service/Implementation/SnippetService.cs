using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<SnippetDto>> GetAll()
    {
        return (await _context.Set<Snippet>().ToListAsync()).Select(x => Map(x));
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
            snippet.Deleted = true;
            _context.Update(snippet);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<SnippetDto>> Search(string keySearch)
    {
        var snippetDtos = _context.Set<Snippet>().Where(x=> !x.Deleted).Include(x => x.Tags).Select(x => new SnippetDto()
        {
            Id = x.Id,
            Name =x.Name,
            Content = x.Content,
            Description = x.Description,
            Origin = x.Origin,
            Created = x.Created,
            Modified = x.Modified,
            Tags = x.Tags.Select(tag => new TagDto()
            {
                Id = tag.TagId,
                TagName = tag.Tag.TagName
            })
        });
        
        var term = keySearch?.ToLower()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        snippetDtos = term.Aggregate(snippetDtos,
            (originalQuery, term) => originalQuery.Where(x => x.Name.ToLower().Contains(keySearch)
                                                              || x.Origin.ToLower().Contains(keySearch)
                                                              || x.Description.ToLower().Contains(keySearch)
                                                              || x.Content.ToLower().Contains(keySearch)
                                                              || x.Tags.Any(tagDto =>
                                                                  tagDto.TagName.ToLower().Contains(keySearch))
                                                              || x.Created.ToString().Contains(keySearch)
                                                              || x.Modified.ToString().Contains(keySearch)));
        return snippetDtos.ToList();
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
        if (tags is null)
            return list;
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