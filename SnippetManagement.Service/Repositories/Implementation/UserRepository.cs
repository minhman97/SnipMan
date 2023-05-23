using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories.Implementation;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(SnippetManagementDbContext context) : base(context)
    {
    }

    public Task<List<User>> GetUsers()
    {
        return _context.Set<User>().ToListAsync();
    }

    public async Task<UserDto> Create(CreateUserRequest request)
    {
        var user = new User()
        {
            Id = new Guid(),
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Created = DateTimeOffset.UtcNow,
        };
        await _context.AddAsync(user);

        await _context.SaveChangesAsync();

        return Map(user);
    }

    public async Task<UserDto> Get(string email)
    {
        return Map(await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email));
    }

    private UserDto Map(User user)
    {
        if (user is null)
            return null;
        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password
        };
    }
}