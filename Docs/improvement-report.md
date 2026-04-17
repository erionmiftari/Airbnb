# Improvement Sprint — Raport

**Projekti:** Airbnb (Console) — menaxhim listings me CSV  
**Qëllimi:** Përmirësime të menduara (jo thjesht “më shumë kod”), me fokus në strukturë, besueshmëri dhe dokumentim.

---

## Përmbledhje: 3 përmirësime të realizuara

| # | Kategoria | Përshkrimi i shkurtër |
|---|-----------|------------------------|
| 1 | **Kod / strukturë** | `ListingValidation` + `TryReadTitleAndPriceFilters` — më pak duplikim, ndarje më e qartë përgjegësish. |
| 2 | **Reliability / validim** | `InvariantCulture` në `TryAdd`/`TryUpdate`; validim `OwnerId > 0`; ndalim filtri `min > max`; `FileRepository` nuk përdor më `Console` direkt. |
| 3 | **Dokumentim** | Përditësim `README.md` dhe `architecture.md` (emra të saktë, rrjedha UI→Service→Repository). |

---

## Përmirësimi 1 — Strukturë dhe heqje duplikimi

**Çka ishte problem më parë?**  
- Rregullat e validimit për title/çmim ishin të përsëritura në `Add` dhe `Update`.  
- `UiList` dhe `UiStats` përsëritnin të njëjtin lexim për filtra (title, min, max).

**Çfarë ndryshova?**  
- Shtova `Services/ListingValidation.cs` me metoda statike (`ValidateTitle`, `ValidatePrice`, `ValidateOwnerId`, `ValidatePriceFilterRange`).  
- Në `Program.cs` shtova `TryReadTitleAndPriceFilters` që përdoret nga listimi dhe statistikat.

**Pse versioni i ri është më i mirë?**  
- Një burim i vetëm i rregullave të biznesit → më pak rrezik për “harresë” kur ndryshon një rregull.  
- UI më i lexueshëm dhe më i lehtë për mirëmbajtje.

---

## Përmirësimi 2 — Reliability, validim dhe ndarje shtresash

**Çka ishte problem më parë?**  
- `OwnerId` mund të ishte `0` ose jo-logjik si identifikues.  
- `min > max` në filtrim sillte rezultat bosh pa mesazh të qartë.  
- `TryParse` në service nuk ishte i lidhur eksplicit me `InvariantCulture`.  
- `FileRepository` shkruante në `Console` kur mungonte file-i — përzierje e shtresës së të dhënave me UI.

**Çfarë ndryshova?**  
- Validim në `ListingService` përmes `ListingValidation`.  
- `TryAdd` / `TryUpdate` përdorin `NumberStyles` + `CultureInfo.InvariantCulture`.  
- `FileRepository` pranon opsional `Action<string>? onMissingFileCreated`; në `Program` kalon `msg => Console.WriteLine(msg)`.

**Pse versioni i ri është më i mirë?**  
- Sjellje më e parashikueshme për numra në mjedise të ndryshme.  
- Mesazhe më të qarta për gabime logjike të filtrit.  
- Repository më i testueshëm dhe më i pastër për ndarjen UI → Data.

---

## Përmirësimi 3 — Dokumentim dhe shpjegueshmëri

**Çka ishte problem më parë?**  
- `architecture.md` përmendte `BookingService` në vend të `ListingService`.  
- `README` nuk reflektonte plotësisht skedarët e rinj dhe udhëzimet për teste.

**Çfarë ndryshova?**  
- Rishkrim i `Docs/architecture.md` me emra të saktë dhe përshkrim të rrjedhës.  
- Përditësim i `README.md` (strukturë, komanda testesh, referencë për dokumentet e audit/improvement).

**Pse versioni i ri është më i mirë?**  
- Më pak konfuzion për lexues të rinj (profesor, koleg, ti pas 2 javësh).  
- Përputhje më e mirë me kodin aktual.

---

## Testet

U shtuan teste për raste kufitare:

- `List_MinPriceGreaterThanMax_ThrowsArgumentException`
- `Add_OwnerIdZero_ThrowsArgumentException`

---

## Çka mbetet ende e dobët në projekt

- **CSV “i plotë”** — për skallëzim më të madh, një bibliotekë CSV do të ishte më e sigurt se parser-i manual.  
- **Booking/User** — modelet ekzistojnë, por nuk janë të integruara në një flow të vetëm me listings.  
- **Siguria** — nuk ka autentifikim; CSV është i hapur në disk.  
- **Performanca** — `GetAll()` + rilexim në çdo operacion është i thjeshtë por jo optimal për shumë të dhëna.

---

## Dorëzimi në GitHub

Ngarko në repo: `docs/project-audit.md`, `docs/improvement-report.md`, dhe kodin e përditësuar, pastaj jep **linkun e repository** në platformën e kursit.
