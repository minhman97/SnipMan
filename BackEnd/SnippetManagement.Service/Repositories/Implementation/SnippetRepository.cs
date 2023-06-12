using Microsoft.EntityFrameworkCore;
using SnippetManagement.Common;
using SnippetManagement.Common.Enum;
using SnippetManagement.Common.Extentions;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories.Implementation;

public class SnippetRepository : BaseRepository<Snippet>, ISnippetRepository
{
    private new readonly SnippetManagementDbContext _context;

    public SnippetRepository(SnippetManagementDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<Snippet?> Find(Guid id)
    {
        return await _context.Set<Snippet>()
            .Include(x => x.Tags)
            .ThenInclude(xx => xx.Tag)
            .SingleOrDefaultAsync(x => x.Id == id && !x.Deleted);
    }

    public async Task<PagedResponse<IEnumerable<SnippetDto?>>> GetAll(Pagination pagination)
    {
        var snippets = (await _context.Set<Snippet>().Where(x => !x.Deleted)
                .Include(x => x.Tags)
                .ThenInclude(xx => xx.Tag)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking().ToListAsync())
            .Select(x => Map(x));
        var totalRecords = await _context.Set<Snippet>().CountAsync();
        return new PagedResponse<IEnumerable<SnippetDto?>>()
        {
            Data = snippets,
            TotalRecords = await _context.Set<Snippet>().CountAsync(),
            TotalPages = (int)Math.Ceiling((double)(totalRecords / pagination.PageSize)),
            PageSize = pagination.PageSize,
            PageNumber = pagination.PageNumber
        };
    }

    public async Task<RangeDataResponse<IEnumerable<SnippetDto>>> GetRange(Guid userId, int startIndex, int endIndex,
        SortOrder sortOrder)
    {
        var query = _context.Set<Snippet>()
            .Where(x => !x.Deleted && x.UserId == userId);
        var totalRecords = await query.CountAsync();
        switch (sortOrder.Property.Capitalize())
        {
            case nameof(Snippet.Created):
                query = sortOrder.OrderWay == OrderWay.Asc
                    ? query.OrderBy(x => x.Created)
                    : query.OrderByDescending(x => x.Created);
                break;
        }

        query = query.Include(x => x.Tags)
            .ThenInclude(xx => xx.Tag)
            .Skip(startIndex)
            .Take(endIndex - startIndex + 1)
            .AsNoTracking();

        return new RangeDataResponse<IEnumerable<SnippetDto>>()
        {
            Data = (await query.ToListAsync()).Select(Map),
            StartIndex = startIndex,
            EndIndex = endIndex,
            TotalRecords = totalRecords
        };
    }

    public async Task<RangeDataResponse<IEnumerable<SnippetDto>>> SearchRange(Guid userId, int startIndex, int endIndex,
        SearchSnippetRequest request, SortOrder sortOrder)
    {
        var query = _context.Set<Snippet>().Where(x => !x.Deleted && x.UserId == userId).Include(x => x.Tags)
            .ThenInclude(x => x.Tag)
            .AsQueryable();
        if (!string.IsNullOrEmpty(request.KeyWord))
        {
            request.KeyWord = request.KeyWord.ToLower();
            query = request.Terms.Aggregate(query,
                (originalQuery, term) => originalQuery.Where(x => x.Name.ToLower().Contains(request.KeyWord)
                                                                  || x.Origin.ToLower().Contains(request.KeyWord)
                                                                  || x.Description.ToLower().Contains(request.KeyWord)
                                                                  || x.Content.ToLower().Contains(request.KeyWord)
                                                                  || x.Tags.Any(tagDto =>
                                                                      tagDto.Tag.TagName.ToLower()
                                                                          .Contains(request.KeyWord))
                                                                  || x.Created.ToString().Contains(request.KeyWord)
                                                                  || x.Modified.ToString().Contains(request.KeyWord)));
        }

        switch (sortOrder.Property.Capitalize())
        {
            case nameof(Snippet.Created):
                query = sortOrder.OrderWay == OrderWay.Asc
                    ? query.OrderBy(x => x.Created)
                    : query.OrderByDescending(x => x.Created);
                break;
        }

        if (request.FromDate is not null)
            query = query.Where(x => x.Created >= request.FromDate);
        if (request.ToDate is not null)
            query = query.Where(x => x.Created <= request.ToDate.Value.AddDays(1));
        var totalRecords = await query.CountAsync();
        var snippets = await query.Skip(startIndex).Take(endIndex - startIndex + 1)
            .AsNoTracking().Select(snippet =>
                new SnippetDto(snippet.Id, snippet.Name, snippet.Content, snippet.Description, snippet.Origin,
                    snippet.Created, snippet.Modified, snippet.Language, snippet.UserId)
                {
                    Tags = snippet.Tags.Select(x => new TagDto(x.TagId, x.Tag.TagName))
                }).ToListAsync();
        return new RangeDataResponse<IEnumerable<SnippetDto>>()
        {
            Data = snippets,
            StartIndex = startIndex,
            EndIndex = endIndex,
            TotalRecords = totalRecords
        };
    }

    public async Task<PagedResponse<IEnumerable<SnippetDto>>> Search(SearchSnippetRequest request)
    {
        var query = _context.Set<Snippet>().Where(x => !x.Deleted).Include(x => x.Tags).ThenInclude(x => x.Tag)
            .AsQueryable();
        if (!string.IsNullOrEmpty(request.KeyWord))
        {
            request.KeyWord = request.KeyWord.ToLower();
            query = request.Terms.Aggregate(query,
                (originalQuery, term) => originalQuery.Where(x => x.Name.ToLower().Contains(request.KeyWord)
                                                                  || x.Origin.ToLower().Contains(request.KeyWord)
                                                                  || x.Description.ToLower().Contains(request.KeyWord)
                                                                  || x.Content.ToLower().Contains(request.KeyWord)
                                                                  || x.Tags.Any(tagDto =>
                                                                      tagDto.Tag.TagName.ToLower()
                                                                          .Contains(request.KeyWord))
                                                                  || x.Created.ToString().Contains(request.KeyWord)
                                                                  || x.Modified.ToString().Contains(request.KeyWord)));
        }

        if (request.TagIds is not null && request.TagIds.Any())
        {
            query = query.Where(x => x.Tags.Any(tag => request.TagIds.Contains(tag.TagId)));
        }

        if (request.FromDate is not null)
            query = query.Where(x => x.Created >= request.FromDate);
        if (request.ToDate is not null)
            query = query.Where(x => x.Created <= request.ToDate.Value.AddDays(1));
        var totalRecords = await query.CountAsync();
        var snippets = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
            .AsNoTracking().Select(snippet => new SnippetDto(snippet.Id, snippet.Name, snippet.Content,
                snippet.Description, snippet.Origin,
                snippet.Created, snippet.Modified, snippet.Language, snippet.UserId)
            {
                Tags = snippet.Tags.Select(x => new TagDto(x.TagId, x.Tag.TagName))
            }).ToListAsync();

        return new PagedResponse<IEnumerable<SnippetDto>>()
        {
            Data = snippets,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize),
            TotalRecords = totalRecords
        };
    }

    public SnippetDto? Map(Snippet? snippet)
    {
        if (snippet is null)
            return null;
        return new SnippetDto(snippet.Id, snippet.Name, snippet.Content, snippet.Description, snippet.Origin,
            snippet.Created, snippet.Modified, snippet.Language, snippet.UserId)
        {
            Tags = MapTag(snippet.Tags)
        };
    }

    private IEnumerable<TagDto>? MapTag(IEnumerable<SnippetTag>? tags)
    {
        if (tags is null)
            return null;
        return tags.Select(x => new TagDto(x.TagId, x.Tag.TagName));
    }
}