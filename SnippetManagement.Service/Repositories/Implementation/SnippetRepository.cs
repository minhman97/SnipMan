using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Repositories.Implementation;


public class SnippetRepository: BaseRepository<Snippet>, ISnippetRepository
{
    private readonly SnippetManagementDbContext _context;
    private ISnippetRepository _snippetRepositoryImplementation;

    public SnippetRepository(SnippetManagementDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Snippet?> Find(Guid id)
    {
        return await _context.Set<Snippet>().Include(x => x.Tags).SingleOrDefaultAsync(x => x.Id == id && !x.Deleted);
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