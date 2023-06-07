using SnippetManagement.DataModel;

namespace SnippetManagement.Service.Repositories;

public interface IRepository<T> where T: BaseEntity<Guid>
{
    void Add(T entity);
    void AddRange(List<T> entities);
    void RemoveRange(List<T> entities);
    void Update(T entity);
    public Task<T> Find(Guid id);
    void Remove(T entity);
}