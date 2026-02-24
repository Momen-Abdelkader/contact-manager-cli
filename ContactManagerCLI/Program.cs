using ContactManagerCLI.Data;
using ContactManagerCLI.Models;
using ContactManagerCLI.Repositories;
using ContactManagerCLI.Services;
using ContactManagerCLI.UI;

namespace ContactManagerCLI;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Enter the JSON file name to load (leave empty for sample data): ");
        var fileName = Console.ReadLine()?.Trim();

        JsonRepository<Contact> repository;

        if (string.IsNullOrEmpty(fileName))
        {
            repository = new JsonRepository<Contact>("contacts.json");
            var seedContacts = ContactSeeder.GetSeedContacts();
            foreach (var contact in seedContacts)
            {
                repository.Add(contact);
            }
            Console.WriteLine($"Loaded {repository.Count} contact(s) from sample data.");
        }
        else
        {
            repository = new JsonRepository<Contact>(fileName);
            await repository.Load();
            Console.WriteLine($"Loaded {repository.Count} contact(s) from '{fileName}'.");
        }

        var service = new ContactService(repository);
        var ui = new ConsoleUi(service);
        await ui.Run();
    }
}