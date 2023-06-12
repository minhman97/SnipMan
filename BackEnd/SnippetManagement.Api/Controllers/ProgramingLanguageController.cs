using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProgramingLanguageController : ControllerBase
{
    [HttpGet]
    public IActionResult GetLanguage()
    {
        return Ok(new List<ProgramingLanguageDto>()
        {
            new("c-sharp", "Assets/Icons/classifications/c-sharp.png"),
            new("html", "Assets/Icons/classifications/html.png"),
            new("css", "Assets/Icons/classifications/css.png")
        });
    }
}