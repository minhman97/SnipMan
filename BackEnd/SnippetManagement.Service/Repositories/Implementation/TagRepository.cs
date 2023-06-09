using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;

public class TagRepository: BaseRepository<Tag>, ITagRepository
{

    public TagRepository(SnippetManagementDbContext context) : base(context)
    {
    }
}