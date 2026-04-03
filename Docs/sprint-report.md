# Sprint 2 Report — Emri Yt

## Çka Përfundova
- Implementova feature të re `Statistika` për listings:
  - numri i itemave
  - totali i çmimeve
  - mesatarja
  - minimumi
  - maksimumi
- Implementova `Sortim` në listim:
  - sipas ID
  - sipas title A-Z
  - sipas çmimit rritës
  - sipas çmimit zbritës
- Përmirësova error handling:
  - file mungon -> krijohet file i ri dhe shfaqet mesazh i qartë
  - input i gabuar numerik -> mesazh i qartë pa crash
  - ID nuk ekziston -> mesazh `Itemi nuk u gjet`
- Krijova projekt testesh `Airbnb.Tests` me teste për:
  - shtim valid
  - shtim me emër bosh (rast kufitar)
  - kërkim/listim për item ekzistues dhe jo-ekzistues
  - llogaritje statistikash

Output i menusë së përditësuar tregon opsionin e ri `Statistika` dhe sortim gjatë listimit.

## Çka Mbeti
- `dotnet test` nuk u ekzekutua për shkak të kufizimit të `PackageSourceMapping` në këtë ambient (paketat xUnit nuk restaurohen nga nuget source aktual).

## Çka Mësova
- Si të dizajnoj feature që kalon qartë nëpër shtresat `UI -> Service -> Repository` pa përzier përgjegjësitë e secilës shtresë.
