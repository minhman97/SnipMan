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
        var token = await _authenticationService.GetToken(new UserDto()
        {
            Email = request.Email,
            Password = request.Password
        });
        if (string.IsNullOrEmpty(token))
        {
            ModelState.AddModelError(string.Empty, "Invalid login");
            return BadRequest(ModelState);
        }

        return Ok(new { token });
    }

    [HttpPost]
    [Route("External", Name = "External")]
    public async Task<IActionResult> GetAuthTokenForExternal([FromBody] string externalToken)
    {
        var token = await _authenticationService.GetTokenForExternalProvider(externalToken);
        if (string.IsNullOrEmpty(token))
        {
            ModelState.AddModelError(string.Empty, "Invalid login");
            return BadRequest(ModelState);
        }

        return Ok(new { token });
    }
}