using ContactManagerCLI.Models;

namespace ContactManagerCLI.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    List<T> GetAll();
    T? GetById(Guid id);
    IEnumerable<T> Find(Func<T, bool> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);
    void SaveAll();
    void Load();
}