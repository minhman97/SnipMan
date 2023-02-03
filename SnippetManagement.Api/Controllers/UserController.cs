using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Api.Model;
using SnippetManagement.Service;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _userService.Get(model.Email) != null)
        {
            ModelState.AddModelError(nameof(UserViewModel.Email),
                "Sorry, this email address is already taken");
            return BadRequest(ModelState);
        }
            
        
        return Ok(await _userService.Create(new CreateUserRequest()
        {
            Email = model.Email,
            Password = model.Password
        }));
    }
}