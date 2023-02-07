namespace SnippetManagement.DataModel;

public class Tag
{
    public Guid Id { get; set; }
    public string TagName { get; set; }

    public IEnumerable<SnippetTag> Snippets { get; set; }
}