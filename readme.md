# Datenbank
Die Daten werden in einer PostgreSQL-Datenbank gespeichert. In der Entwicklungsumgebung wird diese in der docker-compose-yml konfiguriert.

## Datenbankadministration
Über http://localhost:5050 ist PgAdmin4 erreichbar. Damit lässt sich die Datenbank komfortabel verwalten. Der Benutzername lautet zli@example.com und das Passwort zli*123. Die Verbindung zur PostgreSQL-Datenbank muss zuerst mit folgenden Daten konfiguriert werden:

* Host name/address: db
* Port: 5432
* Maintenance database: postgres
* Username: postgres
* Password: postgres

## UML Anwendungsfalldiagramm

```mermaid
usecase-beta
    actor User("User")
    systemBoundary punchClock["Punch Clock"]
        Request("Get all entries from DB")
        Create("Add new entry in DB")
    end
    json Entry@{
        "id": 42,
        "checkin": "2023-01-01T12:00:00",
        "checkout": "2023-01-01T18:30:00"
    }
    User requests@-- "requests all entries" ---> Request
    User adds@-- "adds new entry" --> Create
    Create --> Entry
```

## UML Klassendiagramm

```mermaid
classDiagram
    class Entry {
        +Int Id
        +DateTime CheckIn
        +DateTime CheckOut
    }
    
    class EntryService {
        FindAll() List[Entry]
        Create(entry: Entry) Entry
    }
    
    class EntryController {
        Get() List[Entry]
        AddEntry(entry: Entry) Entry
    }
    
    class DatabaseContext {
        +DbSet[Entry] Entries
    }
    
    EntryController --> EntryService
    EntryController --> Entry
    EntryService --> DatabaseContext
    EntryService --> Entry
    DatabaseContext --> Entry
```

## ERD

```mermaid
erDiagram
    Entry {
        integer Id PK "auto increment"
        integer CategoryId FK
        timestamp CheckIn
        timestamp CheckOut
    }
    
    Category {
        integer Id PK "auto increment"
        varchar(255) Title
    }
    
    Tag {
        integer Id PK "auto increment"
        varchar(255) Title
    }
    
    Entry_Tag {
        integer EntryId PK
        integer TagId PK
    }
    
    Entry }o--o| Category : has
    Entry ||--o{ Entry_Tag : has
    Tag ||--o{ Entry_Tag : has
```

## Sequenzdiagramme

```mermaid
sequenceDiagram
    actor User
    participant Api as Controller
    participant Service
    participant Context as Database Context
    participant DB as Database
    User ->> Api: HTTP GET /entries
    Api ->> Service: FindAll()
    Service ->> Context: .Entries.ToListAsync()
    Context -->> DB: SELECT * FROM Entry
    DB -->> Context: Entry Records
    Context ->> Service: List[Entry]
    Service ->> Api: List[Entry]
    Api ->> User: HTTP 200 OK { entries: [] }
```

```mermaid
sequenceDiagram
    actor User
    participant Api as Controller
    participant Service
    participant Context as Database Context
    participant DB as Database
    User ->> Api: HTTP POST /entries
    Api ->> Service: AddEntry()
    Service ->> Context: .Entries.Add()
    Context -->> DB: INSERT INTO Entry
    Context ->> Service: Entry
    Service ->> Api: Entry
    Api ->> User: HTTP 200 OK { entry: {} }
```
