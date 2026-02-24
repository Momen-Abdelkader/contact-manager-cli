using ContactManagerCLI.Models;

namespace ContactManagerCLI.Services;

public interface IContactService
{
        List<Contact> GetAllContacts();
        Contact? GetContactById(Guid id);
        IEnumerable<Contact> Search(string query);
        IEnumerable<Contact> Filter(Func<Contact, bool> predicate);
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Guid id);
        void SaveAll();
}