namespace SnippetManagement.Api.Model;

public class SnippetViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}