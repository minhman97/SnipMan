using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly SnippetManagementDbContext _context;

    public UserRepository(SnippetManagementDbContext context) : base(context)
    {
        _context = context;
    }
    public Task<List<User>> GetUsers()
    {
        return _context.Set<User>().ToListAsync();
    }
}