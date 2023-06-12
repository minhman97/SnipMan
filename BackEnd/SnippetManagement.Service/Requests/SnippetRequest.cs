using SnippetManagement.Common;

namespace SnippetManagement.Service.Requests;

public class CreateSnippetRequest
{
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public string Language { get; set; }
    public IEnumerable<CreateTagRequest> Tags { get; set; }
}

public class UpdateSnippetRequest : CreateSnippetRequest
{
    public Guid Id { get; set; }
}

public class SearchSnippetRequest : Pagination
{
    public string KeyWord { get; set; }

    public string[] Terms => KeyWord?.ToLower()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }

    public IEnumerable<Guid>? TagIds { get; set; }
}