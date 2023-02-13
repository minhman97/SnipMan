using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SnippetManagement.Api.Model;
using SnippetManagement.Api.Model.Validator;
using SnippetManagement.Service;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SnippetController : ControllerBase
{
    private readonly ISnippetService _snippetService;
    private readonly IValidator<SnippetViewModel> _validator;

    public SnippetController(ISnippetService snippetService, IValidator<SnippetViewModel> validator)
    {
        _snippetService = snippetService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(SnippetViewModel model)
    {
        var result = await _validator.ValidateAsync(model);
        if (!result.IsValid)
            return BadRequest(result.Errors);
        return Ok(await _snippetService.Create(new CreateSnippetRequest()
        {
            Content = model.Content,
            Name = model.Name,
            Description = model.Description,
            Origin = model.Origin
        }));
    }
    
    [HttpGet]

    public async Task<IActionResult> GetAll()
    {
        return Ok(await _snippetService.GetAll());
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> Get(Guid id)
    {
        var snippet = await _snippetService.Get(id);
        if (snippet is null)
            return NotFound();
        return Ok(snippet);
    }

    [HttpPut]
    public async Task<IActionResult> Update(SnippetViewModel model)
    {
        var snippet = await _snippetService.Update(new UpdateSnippetRequest()
        {
            Id = model.Id,
            Content = model.Content,
            Name = model.Name,
            Description = model.Description,
        });
        if (snippet is null)
            return NotFound();

        return Ok(snippet);
    }

    [HttpGet]
    [Route("search/{keySearch}", Name="Search")]
    public async Task<IActionResult> Search(string keySearch)
    {
        return Ok(await _snippetService.Search(keySearch));
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _snippetService.Delete(id);
        return Ok();
    }
}