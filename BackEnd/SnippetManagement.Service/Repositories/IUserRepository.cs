using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<List<User>> GetUsers();
    Task<UserDto> Create(CreateUserRequest request);
    Task<UserDto?> Get(string email);
}