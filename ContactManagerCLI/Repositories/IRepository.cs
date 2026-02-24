namespace ContactManagerCLI.Repositories;

public interface IRepository<T>
{
    List<T> GetAll();
    T? GetById(int id);
    IEnumerable<T> Find(Func<T, bool> predicate);
    void Add(T item);
    void Update(T item);
    void Delete(int id);
    void SaveAll();
}