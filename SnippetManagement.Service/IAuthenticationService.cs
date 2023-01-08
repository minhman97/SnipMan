using SnippetManagement.Service.Model;

namespace SnippetManagement.Service;

public interface IAuthenticationService
{
    Task<string> GetToken(UserCredentials userCredentials);
}