namespace SnippetManagement.Service.Model;

public class TagDto
{
    public TagDto(Guid id, string tagName)
    {
        Id = id;
        TagName = tagName;
    }

    public Guid Id { get; set; }
    public string TagName { get; set; }

    public IEnumerable<SnippetDto>? Snippets { get; set; }
}