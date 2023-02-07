namespace SnippetManagement.Service.Requests;

public class CreateSnippetRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
}