namespace SnippetManagement.Service.Requests;

public class CreateTagRequest
{
    public CreateTagRequest(string tagName)
    {
        TagName = tagName;
    }

    public string TagName { get; set; }
}

public class TagsExisted
{
    public TagsExisted(Guid id, string tagName)
    {
        Id = id;
        TagName = tagName;
    }

    public Guid Id { get; set; }
    public string TagName { get; set; }
}