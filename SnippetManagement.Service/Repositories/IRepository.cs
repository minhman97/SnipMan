namespace SnippetManagement.Service.Repositories;

public interface IRepository<T> where T: class
{
    void Add(T snippet);
    void AddRange(List<T> snippetTags);
    
}