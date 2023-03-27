using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Implementation;

public class SnippetService : ISnippetService
{
    private SnippetManagementDbContext _context;

    public SnippetService(SnippetManagementDbContext context)
    {
        _context = context;
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

    public async Task<List<SnippetDto>> Search(string keyWord)
    {
        var query = GetQueryableSnippetDtos();
        
        var term = keyWord?.ToLower()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        query = term.Aggregate(query,
            (originalQuery, term) => originalQuery.Where(x => x.Name.ToLower().Contains(keyWord)
                                                              || x.Origin.ToLower().Contains(keyWord)
                                                              || x.Description.ToLower().Contains(keyWord)
                                                              || x.Content.ToLower().Contains(keyWord)
                                                              || x.Tags.Any(tagDto =>
                                                                  tagDto.TagName.ToLower().Contains(keyWord))
                                                              || x.Created.ToString().Contains(keyWord)
                                                              || x.Modified.ToString().Contains(keyWord)));
        return await query.ToListAsync();
    }

    private IQueryable<SnippetDto> GetQueryableSnippetDtos()
    {
        return _context.Set<Snippet>().Where(x=> !x.Deleted).Include(x => x.Tags).Select(x => new SnippetDto()
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
    }

    public async Task<List<SnippetDto>> Filter(FilterSnippetRequest request)
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
                                                                      tagDto.TagName.ToLower().Contains(request.KeyWord))
                                                                  || x.Created.ToString().Contains(request.KeyWord)
                                                                  || x.Modified.ToString().Contains(request.KeyWord)));
        }
        //TODO: filter by multiple tag
        if (request.Tags is not null && request.Tags.Any())
        {
            
        }
        
        if (request.FromDate is not null)
            query = query.Where(x => x.Created >= request.FromDate);
        if (request.ToDate is not null)
            query = query.Where(x => x.Created <= request.ToDate.Value.AddDays(1));
        

        return await query.ToListAsync();
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