using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Service;

public interface ISnippetService
{
    Task<SnippetDto> Create(CreateSnippetRequest request);
    Task<SnippetDto> Get(Guid id);
    Task Delete(Guid id);
    Task<SnippetDto> Update(UpdateSnippetRequest request);
    Task<List<SnippetDto>> Search(string keySearch);
    Task<IEnumerable<SnippetDto>> GetAll();
}