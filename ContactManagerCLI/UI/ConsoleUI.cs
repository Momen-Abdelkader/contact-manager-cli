using ContactManagerCLI.Models;
using ContactManagerCLI.Services;
using ContactManagerCLI.Validation;

namespace ContactManagerCLI.UI;

public class ConsoleUI : IUI
{
    private readonly IContactService _contactService;
    private bool _hasUnsavedChanges;

    public ConsoleUI(IContactService contactService)
    {
        _contactService = contactService;
    }

    public async Task Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("----- Contact Manager -----");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. View Contact");
            Console.WriteLine("5. List Contacts");
            Console.WriteLine("6. Search");
            Console.WriteLine("7. Filter");
            Console.WriteLine("8. Save");
            Console.WriteLine("9. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    AddContact();
                    break;
                case "2":
                    EditContact();
                    break;
                case "3":
                    DeleteContact();
                    break;
                case "4":
                    ViewContact();
                    break;
                case "5":
                    ListContacts();
                    break;
                case "6":
                    SearchContacts();
                    break;
                case "7":
                    FilterContacts();
                    break;
                case "8":
                    await SaveContacts();
                    break;
                case "9":
                    if (await ConfirmExit()) return;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private void AddContact()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine()?.Trim();
        var nameError = ContactValidator.ValidateName(name);
        if (nameError is not null)
        {
            Console.WriteLine(nameError);
            return;
        }

        Console.Write("Email: ");
        var email = Console.ReadLine()?.Trim();
        var emailError = ContactValidator.ValidateEmail(email);
        if (emailError is not null)
        {
            Console.WriteLine(emailError);
            return;
        }

        Console.Write("Phone: ");
        var phone = Console.ReadLine()?.Trim();
        var phoneError = ContactValidator.ValidatePhone(phone);
        if (phoneError is not null)
        {
            Console.WriteLine(phoneError);
            return;
        }

        var contact = new Contact(name!, email!, phone!);
        _contactService.AddContact(contact);
        _hasUnsavedChanges = true;
        Console.WriteLine("Contact added successfully.");
        Console.WriteLine($"  {contact}");
    }

    private void EditContact()
    {
        Console.Write("Enter contact ID to edit: ");
        var input = Console.ReadLine()?.Trim();
        if (!Guid.TryParse(input, out var id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var contact = _contactService.GetContactById(id);
        if (contact is null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        Console.WriteLine($"Current Name: {contact.Name}");
        Console.Write("New Name (leave blank to keep): ");
        var name = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(name))
        {
            var nameError = ContactValidator.ValidateName(name);
            if (nameError is not null)
            {
                Console.WriteLine(nameError);
                return;
            }
            contact.Name = name;
        }

        Console.WriteLine($"Current Email: {contact.Email}");
        Console.Write("New Email (leave blank to keep): ");
        var email = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(email))
        {
            var emailError = ContactValidator.ValidateEmail(email);
            if (emailError is not null)
            {
                Console.WriteLine(emailError);
                return;
            }
            contact.Email = email;
        }

        Console.WriteLine($"Current Phone: {contact.PhoneNumber}");
        Console.Write("New Phone (leave blank to keep): ");
        var phone = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(phone))
        {
            var phoneError = ContactValidator.ValidatePhone(phone);
            if (phoneError is not null)
            {
                Console.WriteLine(phoneError);
                return;
            }
            contact.PhoneNumber = phone;
        }

        _contactService.UpdateContact(contact);
        _hasUnsavedChanges = true;
        Console.WriteLine("Contact updated successfully.");
        Console.WriteLine($"  {contact}");
    }

    private void DeleteContact()
    {
        Console.Write("Enter contact ID to delete: ");
        var input = Console.ReadLine()?.Trim();
        if (!Guid.TryParse(input, out var id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        try
        {
            _contactService.DeleteContact(id);
            _hasUnsavedChanges = true;
            Console.WriteLine("Contact deleted successfully.");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Contact not found.");
        }
    }

    private void ViewContact()
    {
        Console.Write("Enter contact ID to view: ");
        var input = Console.ReadLine()?.Trim();
        if (!Guid.TryParse(input, out var id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var contact = _contactService.GetContactById(id);
        if (contact is null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        Console.WriteLine($"  {contact}");
    }

    private void ListContacts()
    {
        var contacts = _contactService.GetAllContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }
        
        Console.WriteLine($"{contacts.Count} contact(s) found.");
        foreach (var contact in contacts)
        {
            Console.WriteLine($"  {contact}");
        }
    }

    private void SearchContacts()
    {
        Console.Write("Enter search query: ");
        var query = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(query))
        {
            Console.WriteLine("Search query cannot be empty.");
            return;
        }

        var results = _contactService.Search(query).ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("No contacts matched your search.");
            return;
        }

        Console.WriteLine($"{results.Count} contact(s) found.");
        foreach (var contact in results)
        {
            Console.WriteLine($"  {contact}");
        }
    }

    private void FilterContacts()
    {
        Console.WriteLine("Filter by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Email");
        Console.WriteLine("3. Phone");
        Console.Write("Choose a field: ");
        var fieldChoice = Console.ReadLine()?.Trim();

        if (fieldChoice is not ("1" or "2" or "3"))
        {
            Console.WriteLine("Invalid field choice.");
            return;
        }
        
        Console.Write("Enter value to filter by: ");
        var value = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(value))
        {
            Console.WriteLine("Filter value cannot be empty.");
            return;
        }

        Func<Contact, bool> predicate = fieldChoice switch
        {
            "1" => c => c.Name.Contains(value, StringComparison.OrdinalIgnoreCase),
            "2" => c => c.Email.Contains(value, StringComparison.OrdinalIgnoreCase),
            "3" => c => c.PhoneNumber.Contains(value, StringComparison.OrdinalIgnoreCase),
            _ => _ => false // do not filter if invalid choice, but this should never happen due to earlier check
        };
        
        var results = _contactService.Filter(predicate).ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("No contacts matched your filter.");
            return;
        }

        Console.WriteLine($"{results.Count} contact(s) found.");
        foreach (var contact in results)
        {
            Console.WriteLine($"  {contact}");
        }
    }

    private async Task<bool> ConfirmExit()
    {
        if (!_hasUnsavedChanges)
        {
            Console.WriteLine("Exiting...");
            return true;
        }

        Console.Write("Save changes before exiting? (y/n): ");
        var answer = Console.ReadLine()?.Trim().ToLower();
        if (answer == "y")
        {
            await _contactService.SaveAll();
            _hasUnsavedChanges = false;
            Console.WriteLine("Changes saved. Exiting...");
        }
        else if (answer == "n")
        {
            Console.WriteLine("Changes discarded. Exiting...");
        }
        else
        {
            Console.WriteLine("Invalid input. Returning to menu.");
            return false;
        }
        return true;
    }

    private async Task SaveContacts()
    {
        await _contactService.SaveAll();
        _hasUnsavedChanges = false;
        Console.WriteLine("Contacts saved successfully.");
    }
}

