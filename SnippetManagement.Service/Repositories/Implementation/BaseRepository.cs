using SnippetManagement.Data;

namespace SnippetManagement.Service.Repositories.Implementation;

public class BaseRepository<T> :IRepository<T> where T : class
{
    private readonly SnippetManagementDbContext _context;

    public BaseRepository(SnippetManagementDbContext context)
    {
        _context = context;
    }
    public void Add(T snippetTag)
    {
        _context.Add(snippetTag);
    }

    public void AddRange(List<T> snippetTags)
    {
        _context.AddRange(snippetTags);
    }
}