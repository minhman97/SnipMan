using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Api.Model;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Services;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> GetAuthToken(LoginRequest request)
    {
        var result = await _authenticationService.GetToken(new UserDto()
        {
            Email = request.Email,
            Password = request.Password
        }, string.Empty);
        if (result.IsFailed)
        {
            return BadRequest(result);
        }

        return Ok(new { token = result.Value });
    }

    [Route("External", Name = "External")]
    [HttpPost]
    public async Task<IActionResult> GetAuthTokenForExternal([FromBody] string externalToken)
    {
        var result = await _authenticationService.GetToken(new UserDto(), externalToken);
        if (result.IsFailed)
        {
            return BadRequest(result);
        }

        return Ok(new { token = result.Value });
    }
}