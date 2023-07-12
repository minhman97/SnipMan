namespace SnippetManagement.Service.Model;

public class SnippetDto
{
    public SnippetDto(Guid id, string name, string content, string description, string origin, DateTimeOffset created, DateTimeOffset? modified, string language, Guid userId)
    {
        Id = id;
        Name = name;
        Content = content;
        Description = description;
        Origin = origin;
        Created = created;
        Modified = modified;
        Language = language;
        UserId = userId;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public string Language { get; set; }
    public Guid UserId { get; set; }
    public UserDto User { get; set; }
    public IEnumerable<TagDto> Tags { get; set; }
}