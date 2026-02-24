namespace ContactManagerCLI.Models;

public class Contact(string name, string email, string phoneNumber)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PhoneNumber { get; set; } = phoneNumber;
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
}