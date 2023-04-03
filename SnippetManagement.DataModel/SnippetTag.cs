namespace SnippetManagement.DataModel;

public class SnippetTag: BaseEntity
{
    public Guid SnippetId { get; set; }
    public Snippet Snippet { get; set; }
    public Guid TagId { get; set; }
    public Tag Tag { get; set; }
}