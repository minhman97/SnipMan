using SnippetManagement.Data;
using SnippetManagement.DataModel;
using Microsoft.EntityFrameworkCore;

namespace SnippetManagement.Service.Repositories.Implementation;

public class SnippetTagRepository: IRepository<SnippetTag>, ISnippetTagRepository
{
    private readonly SnippetManagementDbContext _context;

    public SnippetTagRepository(SnippetManagementDbContext context)
    {
        _context = context;
    }

    public void Add(SnippetTag snippetTag)
    {
        _context.Add(snippetTag);
    }

    public void AddRange(List<SnippetTag> snippetTags)
    {
        _context.AddRange(snippetTags);
    }

    public async Task<IEnumerable<SnippetTag>> GetSnippetTagsBySnippetId(Guid id)
    {
        return await _context.Set<SnippetTag>().Include(x => x.Tag).Include(x=>x.Snippet).Where(x => x.SnippetId == id).AsNoTracking().ToListAsync();
    }
}