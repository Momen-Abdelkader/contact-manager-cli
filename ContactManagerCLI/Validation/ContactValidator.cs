using System.Text.RegularExpressions;

namespace ContactManagerCLI.Validation;

public static class ContactValidator
{
    public static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase
        );
    }

    public static bool IsValidPhone(string phone)
    {
        return Regex.IsMatch(phone, @"^\+?[\d\-\(\)\s]{7,15}$");
    }

    public static string? ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Name cannot be empty.";
        }
        
        return null;
    }

    public static string? ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email cannot be empty.";
        }

        if (!IsValidEmail(email))
        {
            return "Invalid email format.";
        }
        
        return null;
    }

    public static string? ValidatePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return "Phone number cannot be empty.";
        }

        if (!IsValidPhone(phone))
        {
            return "Invalid phone number format.";
        }
        
        return null;
    }
}


