namespace SnippetManagement.DataModel;

public class Snippet: BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public string Language { get; set; }
    public User? User { get; set; }
    public IEnumerable<SnippetTag> Tags { get; set; }

    public Snippet(Guid id, string name, string content, string description, string origin, string language, Guid userId) : base(id)
    {
        Name = name;
        Content = content;
        Description = description;
        Origin = origin;
        Language = language;
        UserId = userId;
    }
}