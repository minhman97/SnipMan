using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;


public class SnippetRepository: BaseRepository<Snippet>, ISnippetRepository
{
    private readonly SnippetManagementDbContext _context;
    public SnippetRepository(SnippetManagementDbContext context) : base(context)
    {
        _context = context;
    }
}