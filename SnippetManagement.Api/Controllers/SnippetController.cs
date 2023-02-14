using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetManagement.Service;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SnippetController : ControllerBase
{
    private readonly ISnippetService _snippetService;

    public SnippetController(ISnippetService snippetService)
    {
        _snippetService = snippetService;
    }
    //write test unit
    [HttpPost]
    public async Task<IActionResult> Create(CreateSnippetRequest request)
    {
        return Ok();
        return Ok(await _snippetService.Create(request));
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateSnippetRequest request)
    {
        return Ok();
        request.Id = id;
        var snippet = await _snippetService.Update(request);
        if (snippet is null)
            return NotFound();

        return Ok(snippet);
    }

    [HttpGet]
    [Route("search/{keyWord}", Name="Search")]
    public async Task<IActionResult> Search(string keyWord)
    {
        return Ok(await _snippetService.Search(keyWord));
    }

    [HttpGet]
    [Route("filter/", Name="Filter")]
    public async Task<IActionResult> Filter([FromQuery]FilterSnippetRequest request)
    {
        return Ok(await _snippetService.Filter(request));
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var snippet = await _snippetService.Get(id);
        if (snippet is null)
            return NotFound();
        await _snippetService.Delete(id);
        return Ok();
    }
}