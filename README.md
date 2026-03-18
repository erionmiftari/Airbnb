# 🏠 Airbnb App

Airbnb App është një aplikacion për menaxhimin dhe rezervimin e banesave, i ndërtuar me **C#**.  
Përdoruesit mund të regjistrohen, të shtojnë banesat e tyre për qira, dhe të rezervojnë banesa të tjera.

> ⚠️ Projekti është në zhvillim aktiv — faza aktuale mbulon backend logjikën (modelet, shërbimet dhe ruajtjen e të dhënave). Frontend-i do të shtohet në fazat e ardhshme.

---

## 📁 Struktura e Projektit

```
Airbnb/
├── Models/
│   ├── User.cs           # Modeli i përdoruesit
│   ├── Listing.cs        # Modeli i banesës
│   └── Booking.cs        # Modeli i rezervimit
├── Services/
│   ├── UserService.cs    # Logjika e përdoruesit
│   └── BookingService.cs # Logjika e rezervimit
├── Data/
│   ├── IRepository.cs    # Interfejsi i repository
│   └── FileRepository.cs # Implementimi i ruajtjes
├── Docs/
│   ├── architecture.md   # Arkitektura e sistemit
│   └── class-diagram.md  # Diagrami i klasave
├── UI/
│   └── Program.cs        # Pika hyrëse e aplikacionit
├── .gitignore
└── README.md
```

---

## ✅ Funksionalitetet (Faza 1 - Backend)

- Regjistrimi dhe menaxhimi i përdoruesve
- Shtimi, modifikimi dhe fshirja e banesave (listings)
- Rezervimi i banesave me validim të disponueshmërisë
- Ruajtja e të dhënave në database (SQL)
- Repository pattern për akses të strukturuar në të dhëna

---

## 🚀 Fazat e Zhvillimit

| Faza | Përshkrimi | Statusi |
|------|-----------|---------|
| Faza 1 | Backend - Modelet, Shërbimet, Database | ✅ Në progres |
| Faza 2 | REST API (ASP.NET Core) | 🔜 E ardhshme |
| Faza 3 | Frontend (React / Blazor) | 🔜 E ardhshme |
| Faza 4 | Authentication & Deployment | 🔜 E ardhshme |

---

## 🛠️ Teknologjitë

- **Gjuha:** C#
- **Database:** SQL
- **Pattern:** Repository Pattern, Separation of Concerns
- **Version Control:** Git + GitHub

---

## ⚙️ Instalimi

```bash
# Klono projektin
git clone https://github.com/erionmiftari/Airbnb.git

# Hyr në folder
cd Airbnb

# Hap me Visual Studio ose VS Code dhe ekzekuto
dotnet run
```

---

## 👤 Autori

**Erion Miftari**  
[GitHub](https://github.com/erionmiftari)
