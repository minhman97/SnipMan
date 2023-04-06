namespace SnippetManagement.Service.Requests;

public class CreateTagRequest
{
    public Guid? Id { get; set; }
    public string TagName { get; set; }
}

public class FilterTag
{
    public string TagName { get; set; }
}