using SnippetManagement.Common.Enum;

namespace SnippetManagement.Service.Requests;

public class CreateUserRequest
{
    public string Email { get; set; }
    public string? Password { get; set; }
    public SocialProvider? SocialProvider { get; set; }
}

