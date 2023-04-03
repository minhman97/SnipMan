namespace SnippetManagement.DataModel;

public class User: BaseEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}