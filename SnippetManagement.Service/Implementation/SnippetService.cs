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
            Tags = _snippetTagService.MapSnippetTag(snippet.Tags)
        };
    }
}