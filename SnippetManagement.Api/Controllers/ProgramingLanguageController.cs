using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Api.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class ProgramingLanguageController : ControllerBase
{
    // GET
    [HttpGet]
    public IActionResult GetLanguage()
    {
        return Ok( new List<ProgramingLanguageDto>()
        {
            new ()
            {
                Name = "c-sharp",
                Url = "Assets/Icons/classifications/c-sharp.png"
            },
            new ()
            {
                Name = "html",
                Url = "Assets/Icons/classifications/c-sharp.png"
            },
            new ()
            {
                Name = "css",
                Url = "Assets/Icons/classifications/c-sharp.png"
            }
        });
    }
}