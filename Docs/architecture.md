# Architecture — Airbnb (Console)

## Qëllimi i dokumentit

Ky dokument përshkruan **shtresat** e aplikacionit dhe **rrjedhën e të dhënave** nga UI deri në ruajtje në file, që të jetë e qartë kush bën çfarë.

---

## Shtresat e projektit

```text
┌─────────────────────────────────────────┐
│  UI (UI/Program.cs)                    │
│  - lexon input nga përdoruesi            │
│  - shfaq mesazhe / rezultate           │
└─────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────┐
│  Service (Services/ListingService.cs)    │
│  - logjika e biznesit (filtrim, sort,   │
│    statistika, validim)                 │
│  - nuk lexon/shkruan file direkt        │
└─────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────┐
│  Repository (Data/FileRepository.cs)   │
│  - implementon IRepository<Listing>    │
│  - lexon/shkruan CSV                    │
└─────────────────────────────────────────┘
                    │
                    ▼
              Data/listings.csv
```

### UI Layer (`UI/Program.cs`)

- Shfaq menunë konsol dhe lexon zgjedhjet.
- Nuk përmban logjikë biznesi (p.sh. filtrimi i listës bëhet përmes `ListingService`).
- Për mesazhin kur mungon CSV, UI kalon një callback te `FileRepository` (nuk duhet të printojë vetë repository-i).

### Services Layer (`Services/`)

- **`ListingService`** — operacionet e listings: `List`, `GetStatistics`, `Add`, `Update`, `Delete`, dhe metodat `TryAdd`/`TryUpdate` për input me string nga UI.
- **`ListingValidation`** (klasë e brendshme statike) — rregulla të përbashkëta validimi për të shmangur duplikimin midis `Add` dhe `Update`.

### Data Layer (`Data/`)

- **`IRepository<T>`** — kontratë për CRUD + `Save()`.
- **`FileRepository`** — implementim CSV për `Listing`; përfshin parsing të thjeshtë me mbështetje për thonjëza në title.

### Models Layer (`Models/`)

- **`Listing`**, **`User`**, **`Booking`** — struktura të të dhënave. Në këtë fazë, flow kryesor i konsolës është përqendruar te `Listing`.

---

## Parimet kryesore

- **Repository Pattern** — `ListingService` varet nga `IRepository<Listing>`, jo nga detajet e file-it.
- **Separation of Concerns** — UI nuk lexon CSV; Repository nuk përmban logjikë biznesi për filtrim/statistika.
- **Testueshmëri** — `ListingService` mund të testohet me një repository in-memory që implementon `IRepository<Listing>`.

---

## Parimet SOLID (përmbledhje)

| Parimi | Si aplikohet këtu |
|--------|-------------------|
| **S** | `ListingService` = biznes; `FileRepository` = ruajtje; modelet = të dhëna. |
| **O** | Mund të shtohet `DatabaseRepository` pa ndryshuar `ListingService` nëse implementon `IRepository<Listing>`. |
| **D** | `ListingService` injektohet me `IRepository<Listing>`, jo me klasë konkrete. |

---

## Shënim për të ardhmen

Një API (ASP.NET Core) do të vendoste të njëjtën logjikë në `ListingService`, duke zëvendësuar vetëm shtresën UI (kontroller + HTTP) pa prekur repository-në nëse kontrata mbetet e njëjtë.
