using FluentResults;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Services;

public interface IAuthenticationService
{
    Task<Result> GetToken(UserDto userDto);
    Task<string> GetTokenForExternalProvider(string externalToken);
}