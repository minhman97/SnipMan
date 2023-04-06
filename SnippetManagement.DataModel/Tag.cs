namespace SnippetManagement.DataModel;

public class Tag: BaseEntity<Guid>
{
    public string TagName { get; set; }

    public IEnumerable<SnippetTag> Snippets { get; set; }
}