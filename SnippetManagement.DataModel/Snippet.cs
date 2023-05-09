namespace SnippetManagement.DataModel;

public class Snippet: BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public string Language { get; set; }
    public IEnumerable<SnippetTag> Tags { get; set; }
}