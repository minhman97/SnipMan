using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Implementation;

public class SnippetTagService : ISnippetTagService
{
    private SnippetManagementDbContext _context;

    public SnippetTagService(SnippetManagementDbContext context)
    {
        _context = context;
    }

    public async Task Create(CreateSnippetTagRequest request)
    {
        await _context.AddAsync(new SnippetTag()
        {
            SnippetId = request.SnippetId,
            TagId = request.TagId
        });
        await _context.SaveChangesAsync();
    }
}