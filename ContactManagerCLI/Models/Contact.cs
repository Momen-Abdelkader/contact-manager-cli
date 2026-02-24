namespace ContactManagerCLI.Models;

public class Contact(string name, string email, string phoneNumber) : BaseEntity
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PhoneNumber { get; set; } = phoneNumber;

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Email)}: {Email}, {nameof(PhoneNumber)}: {PhoneNumber}, {nameof(CreatedAt)}: {CreatedAt:MMMM dd, yyyy hh:mm tt}";
    }
}