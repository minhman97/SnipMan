using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service;

public interface ITagService
{
    TagDto Map(Tag tag);
    Task<TagDto> Create(CreateTagRequest request);
}