namespace SnippetManagement.Service.Requests;

public class CreateTagRequest
{
    public CreateTagRequest(string tagName)
    {
        TagName = tagName;
    }

    public Guid? Id { get; set; }
    public string TagName { get; set; }
}