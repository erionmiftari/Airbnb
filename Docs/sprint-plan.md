# Sprint 2 Plan

## Qëllimi
Implementim i një feature të re funksionale me arkitekturë `UI -> Service -> Repository`, përmirësim i trajtimit të gabimeve, dhe shtim i testeve unit.

## Task-et kryesore
- Shto `Statistika` për listings (total, average, min, max, count).
- Shto `Sortim` të listings (ID, Title A-Z, Price asc/desc).
- Përmirëso error handling në të tre shtresat:
  - Repository: file read/write errors.
  - Service: parsing dhe validime.
  - UI: input errors dhe vazhdim i ekzekutimit.
- Krijo projekt testesh dhe shto minimum 3 teste.

## Kriteret e pranimit
- Feature funksionon nga menu e UI dhe përdor service + repository.
- Programi nuk crash-on në input gabim ose mungesë file.
- Testet e implementuara mbulojnë rast normal dhe raste kufitare.
