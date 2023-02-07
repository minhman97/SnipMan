using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service;

public interface ITagService
{
    Task<TagDto> Create(CreateTagRequest request);
    Task Delete(Guid id);
}