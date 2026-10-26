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
```

## ERD

```mermaid
erDiagram
    Entry {
        integer Id PK "auto increment"
        timestamp CheckIn
        timestamp CheckOut
    }
```

## Sequenzdiagramme

```mermaid
sequenceDiagram
    actor User
    participant Web as Web App
    participant DB as Database
    User ->> Web: GET /entries/get
    Web ->> DB: FindAll()
    DB -->> Web: List[Entry]
    Web ->> User: 200 OK List[Entry]
    User ->> Web: POST /entries/addentry
    Web ->> DB: Create(Entry entry)
    DB -->> Web: Entry
    Web ->> User: 201 Created Entry
```
