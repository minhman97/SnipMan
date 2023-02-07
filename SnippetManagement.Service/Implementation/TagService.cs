using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Implementation;

public class TagService : ITagService
{
    private SnippetManagementDbContext _context;
    private readonly ISnippetTagService _snippetTagService;

    public TagService(SnippetManagementDbContext context, ISnippetTagService snippetTagService)
    {
        _context = context;
        _snippetTagService = snippetTagService;
    }

    public async Task<TagDto> Create(CreateTagRequest request)
    {
        var tag = new Tag()
        {
            TagName = request.TagName
        };

        await _context.AddAsync(tag);
        await _context.SaveChangesAsync();

        return Map(tag);
    }

    public async Task Delete(Guid id)
    {
        var tag = await _context.Set<DataModel.Tag>().FindAsync(id);
        if (tag is not null)
        {
            _context.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }

    private TagDto Map(Tag tag)
    {
        if (tag is null)
            return null;

        return new TagDto()
        {
            Id = tag.Id,
            TagName = tag.TagName,
            Snippets = MapSnippet(tag.Snippets)
        };
    }

    private IEnumerable<SnippetDto> MapSnippet(IEnumerable<SnippetTag> snippetTags)
    {
        var list = new List<SnippetDto>();
        foreach (var snippetTag in snippetTags)
        {
            list.Add(new SnippetDto()
            {
                Id = snippetTag.Snippet.Id,
                Content = snippetTag.Snippet.Content,
                Created = snippetTag.Snippet.Created,
                Modified = snippetTag.Snippet.Modified,
                Description = snippetTag.Snippet.Description,
                Name = snippetTag.Snippet.Name,
                Origin = snippetTag.Snippet.Origin
            });
        }
        return list;
    }
}