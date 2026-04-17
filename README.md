# 🏠 Airbnb App

Airbnb App është një aplikacion për menaxhimin dhe rezervimin e banesave, i ndërtuar me **C#**.  
Përdoruesit mund të shtojnë banesat e tyre për qira dhe të menaxhojnë listings.

> ⚠️ Projekti është në zhvillim aktiv — faza aktuale mbulon backend logjikën (modelet, shërbimet dhe ruajtjen e të dhënave). Frontend-i do të shtohet në fazat e ardhshme.

---

## 📁 Struktura e Projektit
```
Airbnb/
├── Models/
│   ├── User.cs              # Modeli i përdoruesit
│   ├── Listing.cs           # Modeli i banesës
│   └── Booking.cs           # Modeli i rezervimit
├── Services/
│   ├── ListingService.cs    # Logjika e biznesit për listings
│   └── ListingValidation.cs # Rregulla të përbashkëta validimi (DRY)
├── Data/
│   ├── IRepository.cs       # Interfejsi i repository
│   ├── FileRepository.cs    # Implementimi me CSV
│   └── listings.csv         # Të dhënat fillestare
├── Docs/
│   ├── architecture.md      # Arkitektura e sistemit
│   ├── class-diagram.md     # Diagrami i klasave (UML)
│   ├── implementation.md    # Dokumentimi i implementimit
│   ├── sprint-plan.md       # Plani i sprint 2
│   ├── sprint-report.md     # Raporti i sprint 2
│   ├── project-audit.md     # Audit i projektit (dobësi + plan përmirësimi)
│   └── improvement-report.md # Raporti i improvement sprint
├── UI/
│   └── Program.cs           # Pika hyrëse e aplikacionit
├── Airbnb.Tests/            # Projekt xUnit (teste për ListingService)
├── NuGet.Config             # (opsionale) nëse ke PackageSourceMapping — lejon restore të testeve
├── .gitignore
└── README.md
```

---

## ✅ Funksionalitetet

- Shto, modifiko dhe fshi listings (banesa)
- Listo listings me filtrim (title, çmim min/max)
- Sortim sipas ID, Title A-Z, çmimit rritës/zbritës
- Statistika: total, mesatare, min, max, numri i listings
- Ruajtja e të dhënave në CSV file
- Repository Pattern për akses të strukturuar në të dhëna
- Error handling i plotë — programi nuk crash-on kurrë

---

## 🚀 Fazat e Zhvillimit

| Faza | Përshkrimi | Statusi |
|------|-----------|---------|
| Faza 1 | Backend - Modelet, Shërbimet, CSV | ✅ Përfunduar |
| Faza 2 | REST API (ASP.NET Core) | 🔜 E ardhshme |
| Faza 3 | Frontend (React / Blazor) | 🔜 E ardhshme |
| Faza 4 | Authentication & Deployment | 🔜 E ardhshme |

---

## 🛠️ Teknologjitë

- **Gjuha:** C#
- **Ruajtja e të dhënave:** CSV File
- **Pattern:** Repository Pattern, Separation of Concerns
- **Teste:** xUnit
- **Version Control:** Git + GitHub

---

## ⚙️ Instalimi dhe ekzekutimi
```bash
# Klono projektin (zëvendëso me URL-në tënde të GitHub-it)
git clone https://github.com/USERNAME/Airbnb.git

# Hyr në folder
cd Airbnb

# Ndërto (opsionale)
dotnet build

# Ekzekuto aplikacionin (working directory = root i repo-së)
dotnet run
```

**Shënim:** `listings.csv` lexohet nga `Data/listings.csv` relativisht te folder-i ku ekzekuton `dotnet run`. Ekzekuto nga root i projektit që të gjendet `Data/`.

---

## 🧪 Teste
```bash
# Nga root i repo-së
dotnet test Airbnb.Tests/Airbnb.Tests.csproj
```

Nëse `dotnet test` dështon me `PackageSourceMapping`, kontrollo që ekziston `NuGet.Config` në root (ose rregullo `NuGet.Config` global të përdoruesit).

---

## 👤 Autori

**Erion Miftari**  
[GitHub](https://github.com/erionmiftari)