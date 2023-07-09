using SnippetManagement.Common;

namespace SnippetManagement.Service.Requests;

public class CreateSnippetRequest
{
    public CreateSnippetRequest(string name, string content, string description, string origin, string language)
    {
        Name = name;
        Content = content;
        Description = description;
        Origin = origin;
        Language = language;
    }

    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public string Language { get; set; }
    public IEnumerable<ExistedTagRequest> TagsExisted { get; set; } = new List<ExistedTagRequest>();
    public IEnumerable<CreateTagRequest> NewTags { get; set; } = new List<CreateTagRequest>();
}

public class UpdateSnippetRequest : CreateSnippetRequest
{
    public Guid Id { get; set; }

    public UpdateSnippetRequest(Guid id, string name, string content, string description, string origin, string language) : base(name, content, description, origin, language)
    {
        Id = id;
    }
}

public class SearchSnippetRequest : Pagination
{
    public string? KeyWord { get; set; }

    public string[] Terms
    {
        get
        {
            return KeyWord is not null
                ? KeyWord.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                : new string[] { };
        }
    }

    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }

    public IEnumerable<Guid>? TagIds { get; set; }
}