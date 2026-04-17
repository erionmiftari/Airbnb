# Project Audit — Airbnb (Console + CSV)

## 1. Përshkrimi i shkurtër i projektit

**Çka bën sistemi?**  
Aplikacion konsol në C# për menaxhimin e **listings** (banesa/prona) të ruajtura në një file **CSV**. Përdoruesi mund të listojë, kërkojë sipas ID, të shtojë, përditësojë dhe fshijë listings, si dhe të përdorë filtrim, sortim dhe statistika mbi të dhënat.

**Kush janë përdoruesit kryesorë?**  
Në këtë fazë projekti trajtohet si mjet për **pronarin/host-in** që menaxhon ofertat e veta (nuk ka ende përdorues të shumtë me login).

**Cili është funksionaliteti kryesor?**  
**CRUD për listings** me ruajtje në file, plus **kërkim/filtrim** dhe **raporte statistikore** mbi një nëngrup të listings (pas filtrit).

---

## 2. Çka funksionon mirë?

1. **Ndarja e përgjegjësive (UI → Service → Repository)** — `Program.cs` merr input dhe shfaq output; `ListingService` përmban logjikën; `FileRepository` lexon/shkruan CSV.
2. **Repository pattern me `IRepository<T>`** — lejon testim me repository in-memory dhe zëvendësim të lehtë të ruajtjes (p.sh. nga file në databazë).
3. **CSV me escaping bazë** — `FileRepository` trajton thonjëza në title për të shmangur thyerjen e kolonave.
4. **Teste unit (xUnit)** — `ListingService` testohet pa file system duke përdorur një repository in-memory.

---

## 3. Dobësitë e projektit

1. **Repository përzier me UI në një pikë** — kur mungon file-i CSV, `FileRepository` përdor `Console.WriteLine` brenda shtresës së të dhënave; kjo e bën më të vështirë testimin e pastër dhe thyen ndarjen e shtresave.
2. **Validimi i përsëritur** — rregullat për title/çmim janë të dyfishuara midis `Add` dhe `Update`, që rrit rrezikun e gabimeve kur ndryshon një rregull.
3. **Parsimi i numrave në `TryAdd`/`TryUpdate`** — `TryParse` pa `InvariantCulture` mund të sjellë sjellje të ndryshme sipas kulturës së sistemit (edhe pse UI vendos InvariantCulture në fillim).
4. **Filtrimi me min/max pa kontroll** — nëse përdoruesi jep min më të madh se max, rezultati mund të jetë bosh pa mesazh të qartë pse.
5. **Mungon validimi i `OwnerId`** — mund të ruhet `0` ose vlera jo-logjike si ID pronari.
6. **Testet nuk mbulojnë të gjitha rrugët** — p.sh. raste kufitare për filtrimin e çmimit dhe validimin e `OwnerId`.
7. **Dokumentimi (`architecture.md`) ka pasur gabime** — përmendet `BookingService` në vend të `ListingService`, që e bën të paqartë leximin për dikë që hy në projekt.
8. **Siguria** — CSV është i hapur në disk; nuk ka autentifikim, autorizim, as validim kundër input të keqdashëm (p.sh. file shumë i madh).

---

## 4. Tre përmirësime që do t’i implementoj

### Përmirësimi A — Strukturë / heqje duplikimi

| | |
|---|---|
| **Problemi** | Validimi për title/çmim përsëritet në `Add` dhe `Update`; UI përsërit të njëjtin lexim filtrash për listim dhe statistika. |
| **Zgjidhja** | Klasë e brendshme `ListingValidation` me metoda statike; metodë ndihmëse në `Program` (`TryReadTitleAndPriceFilters`) për të njëjtin input. |
| **Pse ka rëndësi** | Një burim i vërtetë i së vërtetës për rregullat e biznesit; ndryshime më të sigurta dhe më pak kod për mirëmbajtje. |

### Përmirësimi B — Reliability / validim

| | |
|---|---|
| **Problemi** | `OwnerId` nuk validohet; min/max mund të jenë në rend të kundërt; `TryParse` në service nuk është i lidhur eksplicit me `InvariantCulture`. |
| **Zgjidhja** | `ValidateOwnerId`, `ValidatePriceFilterRange`; `TryParse` me `CultureInfo.InvariantCulture` në `TryAdd`/`TryUpdate`. |
| **Pse ka rëndësi** | Më pak sjellje “të çuditshme” për përdoruesin dhe më pak bugs në ambiente të ndryshme. |

### Përmirësimi C — Dokumentim / shpjegueshmëri

| | |
|---|---|
| **Problemi** | `README` dhe `architecture.md` nuk pasqyronin plotësisht strukturën aktuale dhe kishin gabime emërtimi. |
| **Zgjidhja** | Përditësim i `README` (strukturë, teste, NuGet nëse ekziston) dhe rishkrim i `architecture.md` me shtresa të qarta dhe emra të saktë. |
| **Pse ka rëndësi** | Onboarding më i shpejtë për vlerësues/instructor dhe për ty pas disa javësh. |

---

## 5. Një pjesë që ende nuk e kupton plotësisht

**Rreshtat e CSV-së me presje brenda fushave dhe raste skajesh** — parser-i aktual mbulon një nëngrup të rasteve (thonjëza), por nuk e kam verifikuar me të gjitha kombinimet e mundshme të CSV-së në prodhim (encoding, rreshta të thyer, fusha bosh). Do të doja një strategji më të qartë (p.sh. bibliotekë CSV) para se të kaloj në volum të dhënash më të madh.
