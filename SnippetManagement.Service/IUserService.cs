using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service;

public interface IUserService
{
    Task<UserCredentials> Create(CreateUserRequest request);
    Task<UserCredentials> Get(string email);
}