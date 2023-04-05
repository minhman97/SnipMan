using SnippetManagement.Data;
using SnippetManagement.DataModel;
using Microsoft.EntityFrameworkCore;

namespace SnippetManagement.Service.Repositories.Implementation;

public class SnippetTagRepository : BaseRepository<SnippetTag>, ISnippetTagRepository
{
    public SnippetTagRepository(SnippetManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SnippetTag>> GetSnippetTagsBySnippetId(Guid id)
    {
        return await _context.Set<SnippetTag>()
            .Include(x => x.Tag)
            .Include(x => x.Snippet)
            .Where(x => x.SnippetId == id)
            .AsNoTracking().ToListAsync();
    }
}