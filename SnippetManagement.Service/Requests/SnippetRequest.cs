using Microsoft.AspNetCore.Mvc;

namespace SnippetManagement.Service.Requests;

public class CreateSnippetRequest
{
    public string Name { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Origin { get; set; }
    public IEnumerable<CreateTagRequest> Tags { get; set; }
}

public class UpdateSnippetRequest : CreateSnippetRequest
{
    public Guid Id { get; set; }
}

public class FilterSnippetRequest
{
    public string? KeyWord { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public IEnumerable<FilterTag>? Tags { get; set; }
    
}

