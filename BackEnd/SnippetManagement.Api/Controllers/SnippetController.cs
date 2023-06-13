using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public SnippetController(IUnitOfWork unitOfWork, IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSnippetRequest request)
    {
        var snippet = new Snippet(Guid.NewGuid(), request.Name, request.Content, request.Description, request.Origin,
            request.Language, _identityService.GetCurrentUserId());

        if (request.Tags != null)
        {
            var newTags = GetNewTags(request.Tags);

            var snippetTags = newTags.Concat(GetExistedTags(request.Tags)).Select(x => new SnippetTag()
            {
                SnippetId = snippet.Id,
                TagId = x.Id
            }).ToList();

            _unitOfWork.TagRepository.AddRange(newTags);
            _unitOfWork.SnippetTagRepository.AddRange(snippetTags);
        }

        _unitOfWork.SnippetRepository.Add(snippet);

        await _unitOfWork.SaveChangesAsync();

        snippet.Tags = await _unitOfWork.SnippetTagRepository.GetSnippetTagsBySnippetId(snippet.Id);

        return Ok(_unitOfWork.SnippetRepository.Map(snippet));
    }

    private List<Tag> GetNewTags(IEnumerable<CreateTagRequest> tags)
    {
        return tags.Where(x => x.Id is null).Select(x => new Tag(Guid.NewGuid(), x.TagName)).ToList();
    }

    private IEnumerable<Tag> GetExistedTags(IEnumerable<CreateTagRequest> tags)
    {
        return tags.Where(x => x.Id is not null).Select(x => new Tag((Guid)x.Id!, x.TagName));
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
    public async Task<IActionResult> Update(Guid id, UpdateSnippetRequest request)
    {
        request.Id = id;
        var snippet = await _unitOfWork.SnippetRepository.Find(request.Id);
        if (snippet is null)
            return NotFound();

        if (snippet.Tags is not null)
            _unitOfWork.SnippetTagRepository.RemoveRange(snippet.Tags.ToList());

        if (request.Tags is not null)
        {
            var newTags = GetNewTags(request.Tags);

            var snippetTags = newTags.Concat(GetExistedTags(request.Tags)).Select(x => new SnippetTag()
            {
                SnippetId = snippet.Id,
                TagId = x.Id
            }).ToList();

            _unitOfWork.TagRepository.AddRange(newTags);
            _unitOfWork.SnippetTagRepository.AddRange(snippetTags);
        }


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
}