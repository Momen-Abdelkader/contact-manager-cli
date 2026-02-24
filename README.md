# Contact Manager CLI

## Contents

- [Overview](#overview)
- [Class Diagram](#class-diagram)
- [How to Run](#how-to-run)

## Overview

A simple command-line contact management app built with .NET 10. Supports CRUD operations, searching, and filtering contacts using JSON for persistance.

**Features:**
- Add, edit, delete, and view contacts
- List all contacts
- Search across name, email, and phone number
- Filter contacts by a specific field
- Load and save contacts from/to a JSON file, saves data to `contacts.json` in the project directory (.json files are not pushed to the repository).

## Class Diagram

```mermaid
classDiagram
    direction TB

    %% ── Models ──
    class BaseEntity {
        +Id: Guid
        +CreatedAt: DateTimeOffset
    }

    class Contact {
        +Name: string
        +Email: string
        +PhoneNumber: string
        +ToString() string
    }

    BaseEntity <|-- Contact

    %% ── Data Layer ──
    class IRepository {
        <<interface>>
        +Count: int
        +GetAll() List~T~
        +GetById(id: Guid) T?
        +Find(predicate: Func~T, bool~) IEnumerable~T~
        +Add(entity: T)
        +Update(entity: T)
        +Delete(id: Guid)
        +SaveAll()
        +Load()
    }

    class JsonRepository {
        -filePath: string
        -entities: Dictionary~Guid, T~
    }

    IRepository <|.. JsonRepository

    %% ── Service Layer ──
    class IContactService {
        <<interface>>
        +GetAllContacts() List~Contact~
        +GetContactById(id: Guid) Contact?
        +Search(query: string) IEnumerable~Contact~
        +Filter(predicate: Func~Contact, bool~) IEnumerable~Contact~
        +AddContact(contact: Contact)
        +UpdateContact(contact: Contact)
        +DeleteContact(id: Guid)
        +SaveAll()
    }

    class ContactService {
        -repository: IRepository~Contact~
    }

    IContactService <|.. ContactService
    ContactService --> IRepository : uses

    %% ── UI Layer ──
    class IUi {
        <<interface>>
        +Run()
    }

    class ConsoleUi {
        -contactService: IContactService
    }

    IUi <|.. ConsoleUi
    ConsoleUi --> IContactService : uses

    %% ── Utilities ──
    class ContactValidator {
        +ValidateName(name: string?) string?$
        +ValidateEmail(email: string?) string?$
        +ValidatePhone(phone: string?) string?$
    }

    class ContactSeeder {
        +GetSeedContacts() List~Contact~$
    }

    ConsoleUi --> ContactValidator : validates input
    ContactSeeder ..> Contact : creates
```

**Notes:**

- `BaseEntity` holds `Id` and `CreatedAt`, both auto-generated on creation. Any entity added to app should inherit from it.
- `IRepository<T>` is the generic interface exposed by the persistance layer, it can store any entity that inherits from `BaseEntity`. This makes swapping different implemenations simple.
- `ContactSeeder` provides sample data when no file is specified to load the data.
- `ContactValidator` uses Regex to validate email and phone input.

## How to Run
> **Prerequisites**: .NET 10 SDK

**1. Clone the repo and navigate to it**
```bash
git clone https://github.com/your-username/ContactManagerCLI.git
cd ContactManagerCLI
```

**2. Build**
```bash
dotnet build
```

**3. Run**
```bash
dotnet run --project ContactManagerCLI/ContactManagerCLI.csproj
```
