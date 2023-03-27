using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;


public class SnippetRepository: IRepository<Snippet>
{
    private readonly SnippetManagementDbContext _context;
    public SnippetRepository(SnippetManagementDbContext context)
    {
        _context = context;
    }
    public void Add(Snippet snippet)
    {
        _context.Add(snippet);
    }

    public void AddRange(List<Snippet> snippets)
    {
        _context.AddRange(snippets);
    }
}