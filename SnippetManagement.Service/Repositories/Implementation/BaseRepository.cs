using SnippetManagement.Data;

namespace SnippetManagement.Service.Repositories.Implementation;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly SnippetManagementDbContext _context;

    public BaseRepository(SnippetManagementDbContext context)
    {
        _context = context;
    }
    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }

    public async Task<T> Find(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }

    public void AddRange(List<T> entities)
    {
        _context.AddRange(entities);
    }

    public void RemoveRange(List<T> entities)
    {
        _context.RemoveRange(entities);
    }
}

public class BaseEntity
{
    public bool IsDeleted { get; set; }
}