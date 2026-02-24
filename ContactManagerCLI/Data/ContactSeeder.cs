using ContactManagerCLI.Models;

namespace ContactManagerCLI.Data;

public static class ContactSeeder
{
    public static List<Contact> GetSeedContacts()
    {
        return
        [
            new("Walter White", "heisenberg@graymatter.com", "505-555-0101"),
            new("Jesse Pinkman", "jesse.pinkman@capncook.com", "505-555-0102"),
            new("Hank Schrader", "hank.schrader@dea.gov", "505-555-0103"),
            new("Gustavo Fring", "gus@lospolloshermanos.com", "505-555-0104"),
            new("Saul Goodman", "saul@bettercallsaul.com", "505-555-0105")
        ];
    }
}

