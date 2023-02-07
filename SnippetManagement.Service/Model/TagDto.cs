namespace SnippetManagement.Service.Model;

public class TagDto
{
    public Guid Id { get; set; }
    public string TagName { get; set; }

    public IEnumerable<SnippetTagDto> Snippets { get; set; }
}