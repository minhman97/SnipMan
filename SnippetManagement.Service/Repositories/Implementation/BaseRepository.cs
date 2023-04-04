using SnippetManagement.Data;
using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories.Implementation;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity<Guid>
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

    public virtual async Task<T> Find(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }

    public void AddRange(List<T> entities)
    {
        _context.AddRange(entities);
    }

    public void Remove(T entity)
    {
        entity.Deleted = true;
    }

    public void RemoveRange(List<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.Deleted = true;
        }
    }
}