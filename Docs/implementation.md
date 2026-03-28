## Implementimi (CRUD real me CSV)

### Çfarë u implementua

- **Modeli kryesor**: `Listing` (`Models/Listing.cs`)
- **Repository**: `FileRepository` (`Data/FileRepository.cs`)
  - **Lexon nga CSV** në `GetAll()`
  - **GetById(id)**, **Add(item)**, **Update(item)**, **Delete(id)** dhe **Save()**
  - File i të dhënave: `Data/listings.csv` (me 5 rekorde fillestare)
- **Service**: `ListingService` (`Services/ListingService.cs`)
  - **List(...)** me filtrim (`titleContains`, `minPrice`, `maxPrice`)
  - **Add(...)** me validim (title jo bosh, price > 0)
  - **GetById(id)**, **Update(...)**, **Delete(id)**
  - Merr `IRepository<Listing>` në konstruktor (Dependency Injection)
- **UI**: Console menu (`UI/Program.cs`)
  - Rrjedha funksionon: **UI → Service → Repository → CSV**

### Output (run real)

```text
--- LISTINGS MENU ---
1. Listo (opsionale: filter title/min/max)
2. Gjej sipas ID
3. Shto Listing
4. Update Listing
5. Delete Listing
0. Exit
Zgjedhja: Title: Price: OwnerId: U shtua me sukses. ID e re: 6

--- LISTINGS MENU ---
Zgjedhja: ID: ID: 6 | Title: New Listing | Price: 99.99 | OwnerId: 201

--- LISTINGS MENU ---
Zgjedhja: ID: Title: Price: OwnerId: U përditësua me sukses.

--- LISTINGS MENU ---
Zgjedhja: ID: U fshi me sukses.

--- LISTINGS MENU ---
Zgjedhja: ID: Nuk u gjet.
```

### Si ta ekzekutoni

```bash
dotnet run
```

