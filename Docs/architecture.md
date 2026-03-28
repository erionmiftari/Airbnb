# Architecture

## Shtresat e Projektit

### UI Layer (UI/Program.cs)
Merret me ndërveprimin me përdoruesin. Vetëm inicializon shërbimin dhe e starton.

### Services Layer (Services/)
Përmban logjikën e aplikacionit. BookingService menaxhon krijimin dhe shfaqjen e të dhënave.

### Data Layer (Data/)
Menaxhon ruajtjen e të dhënave përmes Repository Pattern.
- `IRepository<T>` — interface gjenerik me operacione CRUD
- `FileRepository` — implementim që ruan të dhënat në CSV file (për `Listing`)

### Models Layer (Models/)
Përfaqëson strukturën e të dhënave: User, Listing, Booking.

---

## Arsyet e Vendimeve

- **Repository Pattern** — abstrakton aksesin në të dhëna, kështu nëse ndryshojmë nga file në database, ndryshojmë vetëm FileRepository, jo gjithë kodin
- **Interface IRepository** — lejon testim dhe zëvendësim të lehtë të implementimit
- **Shtresa të ndara** — çdo shtresë ka një përgjegjësi të vetme, kodi është më i qartë

---

## Parimet SOLID të Aplikuara

### S — Single Responsibility Principle ✅
Çdo klasë ka një përgjegjësi të vetme:
- `User`, `Listing`, `Booking` — vetëm ruajnë të dhëna
- `ListingService` — vetëm logjika e biznesit
- - `FileRepository` — vetëm ruajtja e të dhënave

### O — Open/Closed Principle ✅
`FileRepository` implementon `IRepository`. Nëse duam `DatabaseRepository`, krijojmë klasë të re pa ndryshuar kodin ekzistues.

### D — Dependency Inversion Principle ✅
`ListingService` varet nga `IRepository<Listing>` (interface), jo nga `FileRepository` direkt.