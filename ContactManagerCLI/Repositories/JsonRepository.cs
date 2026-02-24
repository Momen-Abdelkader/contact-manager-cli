using System.Text.Json;
using ContactManagerCLI.Models;

namespace ContactManagerCLI.Repositories;

public class JsonRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly string _filePath;
    private Dictionary<Guid, T> _entities = new();

    private static readonly JsonSerializerOptions JsonOptions = new(){ WriteIndented = true };

    public int Count => _entities.Count;

    public JsonRepository(string filePath)
    {
        _filePath = filePath;
    }

    public List<T> GetAll() => _entities.Values.ToList();

    public T? GetById(Guid id) => _entities.GetValueOrDefault(id);

    public IEnumerable<T> Find(Func<T, bool> predicate) => _entities.Values.Where(predicate);

    public void Add(T entity)
    {
        if (!_entities.TryAdd(entity.Id, entity))
        {
            throw new ArgumentException($"{typeof(T).Name} with id '{entity.Id}' already exists.");
        }
    }

    public void Update(T entity)
    {
        if (!_entities.ContainsKey(entity.Id))
        {
            throw new KeyNotFoundException($"{typeof(T).Name} with id '{entity.Id}' not found.");
        }

        _entities[entity.Id] = entity;
    }

    public void Delete(Guid id)
    {
        if (!_entities.Remove(id))
        {
            throw new KeyNotFoundException($"{typeof(T).Name} with id '{id}' not found.");
        }
    }

    public async Task SaveAll()
    {
        var json = JsonSerializer.Serialize(_entities.Values.ToList(), JsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task Load()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }

        var json = await File.ReadAllTextAsync(_filePath);
        var list = JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? [];
        _entities = list.ToDictionary(Entity => Entity.Id);
    }
}