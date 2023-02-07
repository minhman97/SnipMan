namespace SnippetManagement.Service.Requests;

public class CreateSnippetRequest
{
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
}

public class UpdateSnippetRequest : CreateSnippetRequest
{
    public Guid Id { get; set; }
}

