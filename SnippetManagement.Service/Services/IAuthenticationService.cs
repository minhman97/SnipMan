using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Services;

public interface IAuthenticationService
{
    Task<string> GetToken(UserDto userDto);
    Task<string> GetTokenForExternalProvider(string externalToken);
}