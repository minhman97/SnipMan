using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Api.Model;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
   public IActionResult GetAuthToken(UserCredentials userCredentials)
   {
      return Ok();
   }
}