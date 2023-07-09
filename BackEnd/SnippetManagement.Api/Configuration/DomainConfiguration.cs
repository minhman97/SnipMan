using System.ComponentModel.DataAnnotations;

namespace SnippetManagement.Api.Configuration;

public class DomainConfiguration
{
    public DomainConfiguration()
    {
    }
    [Required]
    public string ShareSnippetUrl { get; set; }
    
}