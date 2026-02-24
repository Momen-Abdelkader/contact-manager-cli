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
- Non-blocking I/O for loading and saving data
- Persist contacts to JSON files in the executable directory (e.g., `bin/Debug/net10.0/contacts.json`)

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
        +SaveAll() Task
        +Load() Task
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
        +SaveAll() Task
    }

    class ContactService {
        -repository: IRepository~Contact~
    }

    IContactService <|.. ContactService
    ContactService --> IRepository : uses

    %% ── UI Layer ──
    class IUI {
        <<interface>>
        +Run() Task
    }

    class ConsoleUI {
        -contactService: IContactService
        -hasUnsavedChanges: bool
    }

    IUI <|.. ConsoleUI
    ConsoleUI --> IContactService : uses

    %% ── Utilities ──
    class ContactValidator {
        +ValidateName(name: string?) string?$
        +ValidateEmail(email: string?) string?$
        +ValidatePhone(phone: string?) string?$
    }

    class ContactSeeder {
        +GetSeedContacts() List~Contact~$
    }

    ConsoleUI --> ContactValidator : validates input
    ContactSeeder ..> Contact : creates
```

**Design Principles:**
- **Dependency Inversion** — layers depend on abstractions (`IRepository`, `IContactService`, `IUI`) not implementations
- **Single Responsibility** — each class has one clear purpose (repository for persistence, service for business logic, UI for presentation)
- **Open/Closed** — new entities can be added by extending `BaseEntity` without modifying `JsonRepository<T>`; new repository types (e.g., SQL, XML) can be swapped in without affecting other layers by implementing `IRepository`.
- **Separation of Concerns** — data, business logic, and presentation are cleanly separated

**Notes:**

- `BaseEntity` holds `Id` and `CreatedAt`, both auto-generated on creation. Any entity added to app should inherit from it.
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
