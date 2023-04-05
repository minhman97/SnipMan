using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories.Implementation;

public class SnippetRepository : BaseRepository<Snippet>, ISnippetRepository
{
    private readonly SnippetManagementDbContext _context;

    public SnippetRepository(SnippetManagementDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Snippet?> Find(Guid id)
    {
        return await _context.Set<Snippet>()
            .Include(x => x.Tags)
            .ThenInclude(xx => xx.Tag)
            .SingleOrDefaultAsync(x => x.Id == id && !x.Deleted);
    }

    public async Task<IEnumerable<SnippetDto?>> GetAll()
    {
        return (await _context.Set<Snippet>()
                .Include(x => x.Tags)
                .ThenInclude(xx => xx.Tag)
                .AsNoTracking().ToListAsync())
            .Select(x => Map(x));
    }

    public async Task<List<SnippetDto>> Search(FilterSnippetRequest request)
    {
        var query = GetQueryableSnippetDtos();
        if (!string.IsNullOrEmpty(request.KeyWord))
        {
            var term = request.KeyWord?.ToLower()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            query = term.Aggregate(query,
                (originalQuery, term) => originalQuery.Where(x => x.Name.ToLower().Contains(request.KeyWord)
                                                                  || x.Origin.ToLower().Contains(request.KeyWord)
                                                                  || x.Description.ToLower().Contains(request.KeyWord)
                                                                  || x.Content.ToLower().Contains(request.KeyWord)
                                                                  || x.Tags.Any(tagDto =>
                                                                      tagDto.TagName.ToLower()
                                                                          .Contains(request.KeyWord))
                                                                  || x.Created.ToString().Contains(request.KeyWord)
                                                                  || x.Modified.ToString().Contains(request.KeyWord)));
        }

        if (request.TagIds is not null && request.TagIds.Any())
        {
            query = query.Where(x => x.Tags.Any(tag => request.TagIds.Contains(tag.Id)));
        }

        if (request.FromDate is not null)
            query = query.Where(x => x.Created >= request.FromDate);
        if (request.ToDate is not null)
            query = query.Where(x => x.Created <= request.ToDate.Value.AddDays(1));


        return await query.ToListAsync();
    }

    private IQueryable<SnippetDto> GetQueryableSnippetDtos()
    {
        return _context.Set<Snippet>().Where(x => !x.Deleted).Include(x => x.Tags).Select(x => Map(x));
    }

    public SnippetDto? Map(Snippet snippet)
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
        if (tags is null)
            return null;
        return tags.Select(x => new TagDto()
        {
            Id = x.TagId,
            TagName = x.Tag.TagName
        });
    }
}