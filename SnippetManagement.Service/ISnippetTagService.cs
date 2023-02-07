using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service;

public interface ISnippetTagService
{
    IEnumerable<SnippetTagDto> MapSnippetTag(IEnumerable<SnippetTag> snippetTags);
    Task Create(CreateSnippetTagRequest request);
}