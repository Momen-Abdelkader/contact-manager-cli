using ContactManagerCLI.Models;
using ContactManagerCLI.Repositories;

namespace ContactManagerCLI.Services;

public class ContactService : IContactService
{
    private readonly IRepository<Contact> _repository;

    public ContactService(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public List<Contact> GetAllContacts() => _repository.GetAll();

    public Contact? GetContactById(Guid id) => _repository.GetById(id);

    public IEnumerable<Contact> Search(string query)
    {
        return _repository.Find(c =>
                                c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                c.Email.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                c.PhoneNumber.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Contact> Filter(Func<Contact, bool> predicate) => _repository.Find(predicate);

    public void AddContact(Contact contact) => _repository.Add(contact);

    public void UpdateContact(Contact contact) => _repository.Update(contact);

    public void DeleteContact(Guid id) => _repository.Delete(id);

    public async Task SaveAll() => await _repository.SaveAll();
}

