namespace SnippetManagement.DataModel;
public class BaseEntity<TId>
{
    public TId Id { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public bool Deleted { get; set; }
}