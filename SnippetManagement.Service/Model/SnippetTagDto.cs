namespace SnippetManagement.Service.Model;

public class SnippetTagDto
{
    public Guid SnippetId { get; set; }
    public SnippetDto Snippet { get; set; }
    public Guid TagId { get; set; }
    public TagDto Tag { get; set; }
}