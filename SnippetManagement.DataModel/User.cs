namespace SnippetManagement.DataModel;

public class User: BaseEntity<Guid>
{
    public string Email { get; set; }
    public string Password { get; set; }
}