namespace SnippetManagement.Service.Requests;

public class CreateSnippetTagRequest
{
    public Guid SnippetId { get; set; }
    public Guid TagId { get; set; }
}