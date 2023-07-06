using FluentResults;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Service.Services;

public interface IAuthenticationService
{
    Task<Result<string>> GetToken(UserDto userDto, bool isExternal);
}