using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;

public class UserRepository: IRepository<User>
{
    private readonly SnippetManagementDbContext _context;
    public UserRepository(SnippetManagementDbContext context)
    {
        _context = context;
    }
    
    public void Add(User user)
    {
        _context.Add(user);
    }
    public void AddRange(List<User> users)
    {
        _context.AddRange(users);
    }

    public Task<List<User>> GetUsers()
    {
        return _context.Set<User>().ToListAsync();
    }
}