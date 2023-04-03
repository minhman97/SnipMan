namespace SnippetManagement.DataModel;

public class Tag: BaseEntity
{
    public Guid Id { get; set; }
    public string TagName { get; set; }

    public IEnumerable<SnippetTag> Snippets { get; set; }
}