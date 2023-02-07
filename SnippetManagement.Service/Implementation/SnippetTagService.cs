using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Implementation;

public class SnippetTagService : ISnippetTagService
{
    private SnippetManagementDbContext _context;
    private readonly ISnippetService _snippetService;
    private readonly ITagService _tagService;

    public SnippetTagService(SnippetManagementDbContext context, ISnippetService snippetService, ITagService tagService)
    {
        _context = context;
        _snippetService = snippetService;
        _tagService = tagService;
    }

    public async Task Create(CreateSnippetTagRequest request)
    {
        await _context.AddAsync(new SnippetTag()
        {
            SnippetId = request.SnippetId,
            TagId = request.TagId
        });
        await _context.SaveChangesAsync();
    }
    
    public IEnumerable<SnippetTagDto> MapSnippetTag(IEnumerable<SnippetTag> snippetTags)
    {
        if (snippetTags is null)
            return null;
        var list = new List<SnippetTagDto>();
        foreach (var snippetTag in snippetTags)
        {
            list.Add(new SnippetTagDto()
            {
                SnippetId = snippetTag.SnippetId,
                TagId = snippetTag.TagId,
                Tag = _tagService.Map(snippetTag.Tag),
                Snippet = _snippetService.Map(snippetTag.Snippet)
            });
        }
        return list;
    }
}