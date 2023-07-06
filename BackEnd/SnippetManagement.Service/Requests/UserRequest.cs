using SnippetManagement.Common.Enum;

namespace SnippetManagement.Service.Requests;

public class CreateUserRequest
{
    public CreateUserRequest(string email, string? password, SocialProvider? socialProvider)
    {
        Email = email;
        Password = password;
        SocialProvider = socialProvider;
    }

    public string Email { get; set; }
    public string? Password { get; set; }
    public SocialProvider? SocialProvider { get; set; }
}

