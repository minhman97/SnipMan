using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAuthToken(UserCredentials userCredentials)
    {
        var token = await _authenticationService.GetToken(userCredentials);
        if (string.IsNullOrEmpty(token))
        {
            ModelState.AddModelError(string.Empty, "Invalid login");
            return BadRequest(ModelState);
        }

        return Ok(new {token});
    }
}