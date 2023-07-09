using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SnippetManagement.Api.Configuration;
using SnippetManagement.Api.Service;
using SnippetManagement.Common;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Repositories;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SnippetController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly DomainConfiguration _domainConfiguration;

    public SnippetController(IUnitOfWork unitOfWork, IIdentityService identityService, IOptions<DomainConfiguration> domainConfiguration)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
        _domainConfiguration = domainConfiguration.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSnippetRequest request)
    {
        var snippet = new Snippet(Guid.NewGuid(), request.Name, request.Content, request.Description, request.Origin,
            request.Language, _identityService.GetCurrentUserId());


        var newTags = GetNewTags(request.NewTags);
        var snippetTags = newTags.Concat(GetExistedTags(request.TagsExisted)).Select(x => new SnippetTag()
        {
            SnippetId = snippet.Id,
            TagId = x.Id
        }).ToList();

        _unitOfWork.TagRepository.AddRange(newTags);
        _unitOfWork.SnippetTagRepository.AddRange(snippetTags);
        _unitOfWork.SnippetRepository.Add(snippet);

        await _unitOfWork.SaveChangesAsync();

        snippet.Tags = await _unitOfWork.SnippetTagRepository.GetSnippetTagsBySnippetId(snippet.Id);


        return Ok(_unitOfWork.SnippetRepository.Map(snippet));
    }

    private List<Tag> GetNewTags(IEnumerable<CreateTagRequest> tags)
    {
        return tags.Select(x => new Tag(Guid.NewGuid(), x.TagName)).ToList();
    }

    private IEnumerable<Tag> GetExistedTags(IEnumerable<ExistedTagRequest> tags)
    {
        return tags.Select(x => new Tag(x.Id, x.TagName));
    }

    [HttpGet]
    public async Task<IActionResult> GetSnippets(int startIndex, int endIndex, [FromQuery] SortOrder sortOrder)
    {
        return Ok(await _unitOfWork.SnippetRepository.GetRange(_identityService.GetCurrentUserId(), startIndex,
            endIndex, sortOrder));
    }

    [HttpGet]
    [Route("Search", Name = "Search")]
    public async Task<IActionResult> Search(int startIndex, int endIndex, [FromQuery] SearchSnippetRequest request,
        [FromQuery] SortOrder sortOrder)
    {
        return Ok(await _unitOfWork.SnippetRepository.SearchRange(_identityService.GetCurrentUserId(), startIndex,
            endIndex, request, sortOrder));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var snippet = await _unitOfWork.SnippetRepository.Find(id);
        if (snippet is null)
            return NotFound();
        return Ok(_unitOfWork.SnippetRepository.Map(snippet));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateSnippetRequest request)
    {
        var snippet = await _unitOfWork.SnippetRepository.Find(request.Id);
        if (snippet is null)
            return NotFound();

        _unitOfWork.SnippetTagRepository.RemoveRange(snippet.Tags.ToList());

        var newTags = GetNewTags(request.NewTags);
        var snippetTags = newTags.Concat(GetExistedTags(request.TagsExisted)).Select(x => new SnippetTag()
        {
            SnippetId = snippet.Id,
            TagId = x.Id
        }).ToList();

        _unitOfWork.TagRepository.AddRange(newTags);
        _unitOfWork.SnippetTagRepository.AddRange(snippetTags);

        snippet.Name = request.Name;
        snippet.Content = request.Content;
        snippet.Description = request.Description;
        snippet.Modified = DateTimeOffset.UtcNow;
        snippet.Origin = request.Origin;
        _unitOfWork.SnippetRepository.Update(snippet);

        await _unitOfWork.SaveChangesAsync();

        snippet.Tags = await _unitOfWork.SnippetTagRepository.GetSnippetTagsBySnippetId(snippet.Id);
        return Ok(_unitOfWork.SnippetRepository.Map(snippet));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var snippet = await _unitOfWork.SnippetRepository.Find(id);
        if (snippet is null)
            return NotFound();
        _unitOfWork.SnippetRepository.Remove(snippet);
        await _unitOfWork.SaveChangesAsync();
        return Ok(new { success = true, message = "Snippet:" + snippet.Name + " deleted successfully" });
    }
    
    [HttpGet]
    [Route("GetShareableLink")]
    public async Task<IActionResult> GetShareableLink(Guid snippetId, Guid userId)
    {
        var snippet = await _unitOfWork.SnippetRepository.Find(snippetId);
        if (snippet is null)
            return NotFound();

        if (snippet.ShareableId is null)
        {
            snippet.ShareableId = Guid.NewGuid();
            await _unitOfWork.SaveChangesAsync();
        }
        return Ok(new { ShareableLink = $"{_domainConfiguration.ShareSnippetUrl}?userId={snippet.UserId}&shareableId={snippet.ShareableId}" });
    }

    [HttpGet]
    [Route("Share")]
    [AllowAnonymous]
    public async Task<IActionResult> Share(Guid userId, Guid shareableId)
    {
        return Ok(await _unitOfWork.SnippetRepository.GetShareableSnippet(userId, shareableId));
    }
}