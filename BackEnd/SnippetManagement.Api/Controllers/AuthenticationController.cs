using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Api.Model;
using SnippetManagement.Common.Enum;
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
        var result = await _authenticationService.GetToken(new UserDto(request.Email)
        {
            Password = request.Password
        }, false);
        if (result.IsFailed)
        {
            return BadRequest(new { message = result.Reasons.FirstOrDefault() });
        }

        return Ok(new { token = result.Value });
    }

    [Route("External", Name = "External")]
    [HttpPost]
    public async Task<IActionResult> GetAuthTokenForExternal([FromBody] string externalToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(externalToken);
        var result =
            await _authenticationService.GetToken(new UserDto(jwtToken.Payload["email"].ToString())
            {
                SocialProvider = SocialProvider.Google
            }, true);
        if (result.IsFailed)
        {
            return BadRequest(new { message = result.Reasons.FirstOrDefault() });
        }

        return Ok(new { token = result.Value });
    }
}