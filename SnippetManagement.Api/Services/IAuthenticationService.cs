using SnippetManagement.Api.Model;

namespace SnippetManagement.Api.Services;

public interface IAuthenticationService
{
    Task<string> GetToken(UserCredentials userCredentials);
}