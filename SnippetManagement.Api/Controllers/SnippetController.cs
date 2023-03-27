using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetManagement.DataModel;
using SnippetManagement.Service;
using SnippetManagement.Service.Repositories;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SnippetController : ControllerBase
{
    private readonly ISnippetService _snippetService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISnippetTagRepository _snippetTagRepository;

    public SnippetController(ISnippetService snippetService, IUnitOfWork unitOfWork, ISnippetTagRepository snippetTagRepository)
    {
        _snippetService = snippetService;
        _unitOfWork = unitOfWork;
        _snippetTagRepository = snippetTagRepository;
    }
    //TODO: write unit test
    [HttpPost]
    public async Task<IActionResult> Create(CreateSnippetRequest request)
    {
        var snippet = new Snippet()
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            Name = request.Name,
            Description = request.Description,
            Origin = request.Origin,
        };
        
        var newTags = request.Tags.Where(x => x.Id is null).Select(x => new Tag()
        {
            Id = Guid.NewGuid(),
            TagName = x.TagName
        }).ToList();

        var snippetTags = newTags.Concat(ExistedTags(request)).Select(x => new SnippetTag()
        {
            SnippetId = snippet.Id,
            TagId = x.Id
        }).ToList();
        
        _unitOfWork.SnippetRepository.Add(snippet);
        _unitOfWork.TagRepository.AddRange(newTags);
        _unitOfWork.SnippetTagRepository.AddRange(snippetTags);
        
        await _unitOfWork.SaveChangesAsync();

        snippet.Tags = await _snippetTagRepository.GetSnippetTagsBySnippetId(snippet.Id);

        return Ok(_unitOfWork.Map(snippet));
    }

    private IEnumerable<Tag> ExistedTags(CreateSnippetRequest request)
    {
        return request.Tags.Where(x => x.Id is not null).Select(x => new Tag()
        {
            Id = (Guid)x.Id,
            TagName = x.TagName
        });
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