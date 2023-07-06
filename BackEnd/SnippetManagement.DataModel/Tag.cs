namespace SnippetManagement.DataModel;

public class Tag: BaseEntity<Guid>
{
    public string TagName { get; set; }

    public IEnumerable<SnippetTag> Snippets { get; set; }

    public Tag(Guid id, string tagName) : base(id)
    {
        TagName = tagName;
    }
}