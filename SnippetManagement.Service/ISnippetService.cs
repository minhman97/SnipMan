using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service;

public interface ISnippetService
{
    SnippetDto Map(Snippet snippet);
    Task<SnippetDto> Create(CreateSnippetRequest request);
    Task<SnippetDto> Get(Guid id);
    Task Delete(Guid id);
}