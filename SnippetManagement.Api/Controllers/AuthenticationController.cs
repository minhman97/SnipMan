using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Service;
using SnippetManagement.Service.Model;

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
      return Ok(await _authenticationService.GetToken(new UserCredentials() { UserName = "a", Password = "a" }));
   }
}