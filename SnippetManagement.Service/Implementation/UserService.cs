using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Implementation;

public class UserService : IUserService
{
    private SnippetManagementDbContext _context;

    public UserService(SnippetManagementDbContext context)
    {
        _context = context;
    }

    public async Task<UserCredentials> Create(CreateUserRequest request)
    {
        var user = new User()
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };
        await _context.AddAsync(user);

        await _context.SaveChangesAsync();

        return Map(user);
    }

    public async Task<UserCredentials> Get(string email)
    {
        return Map(await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email));
    }

    private UserCredentials Map(User user)
    {
        if (user is null)
            return null;
        return new UserCredentials()
        {
            Email = user.Email,
            Password = user.Password
        };
    }
}