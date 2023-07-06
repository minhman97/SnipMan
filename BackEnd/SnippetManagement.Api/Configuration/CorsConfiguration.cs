using System.ComponentModel.DataAnnotations;

namespace SnippetManagement.Api.Configuration;

public class CorsConfiguration
{
    public CorsConfiguration()
    {
    }
    [Required]
    public string AllowOrigins { get; set; }
}