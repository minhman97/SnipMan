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

    public TagDto Map(Tag tag)
    {
        if (tag is null)
            return null;

        return new TagDto()
        {
            Id = tag.Id,
            TagName = tag.TagName,
            Snippets = _snippetTagService.MapSnippetTag(tag.Snippets)
        };
    }
}