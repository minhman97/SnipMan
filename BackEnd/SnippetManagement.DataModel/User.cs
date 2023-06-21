using SnippetManagement.Common.Enum;

namespace SnippetManagement.DataModel;

public class User : BaseEntity<Guid>
{
    public User(string email) : base(Guid.NewGuid())
    {
        Email = email;
    }

    public string Email { get; set; }
    public string? Password { get; set; }
    public SocialProvider? SocialProvider { get; set; }

    public IEnumerable<Snippet> Snippets { get; set; }
}