using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;

public class TagRepository: BaseRepository<Tag>, ITagRepository
{
    private new readonly SnippetManagementDbContext _context;

    public TagRepository(SnippetManagementDbContext context) : base(context)
    {
        _context = context;
    }
}