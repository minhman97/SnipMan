using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;

public class TagRepository: IRepository<Tag>
{
    private readonly SnippetManagementDbContext _context;

    public TagRepository(SnippetManagementDbContext context)
    {
        _context = context;
    }
    
    public void Add(Tag tag)
    {
        _context.Add(tag);
    }

    public void AddRange(List<Tag> tags)
    {
        _context.AddRange(tags);
    }
}