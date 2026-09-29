# Prosjekt Kartverket/Heimevernet
  Prosjekt/løsning laget av gruppe 12

  ### Gruppen består av:
  Emil Rode - emilro@uia.no,
  Andreas Hjort - andreasnh@uia.no,
  Sigve Sørensen - sigves@uia.no,
  Leart Hasanaj - learth@uia.no,
  Aryan Sarvarsen - aryans@uia.no,
  Bjørn Tore Busk - bjorntb@uia.no

  ## Systemarkitektur

  Appen er laget i ASP.NET Core MVC (.NET 10) og kjøres via .NET Aspire.

  Kontrollere:
  - Home (forside, personvern, feilside)
  - Behov (opprette/redigere/slette behov og se oversikt, med viktighet og posisjon i kart)
  - Ressurs (opprette/redigere/slette ressurser og se oversikt, med posisjon i kart)
  - Position (registrere geografiske hendelser med kart og radius)
  - KriseInformasjon og Innstillinger (foreløpig bare placeholder-sider)

  Kart er laget med Leaflet. Klikker du i kartet fylles koordinatene ut automatisk.

  Aspire starter både appen og en MariaDB-database. Foreløpig er det bare forsiden som kobler seg til databasen, og den viser bare om tilkoblingen funker eller ikke. Behov, ressurser og hendelser lagres
  fortsatt bare i minnet mens appen kjører, og forsvinner når den restartes.

  ## Drift

  Du trenger Docker Desktop oppe for at databasen skal starte.

  Første gang må du sette et passord til databasen: **Gruppe12!**
  ```
  dotnet user-secrets set Parameters:password <passord> --project Prosjekt/Prosjekt.AppHost
  ```

  Kjøre lokalt:
  ```
  dotnet run --project Prosjekt/Prosjekt.AppHost
  ```

  Vi har også en docker-compose som kobler appen mot en MariaDB-container, men den må kjøre på et eget nettverk (appnet) som må settes opp separat.

  Ting som mangler:
  - Databasen brukes bare på forsiden, ingenting blir lagret i den enda
  - Ingen egen 404-side

  Vanlig arbeidsflyt i gruppa: hver lager sin egen branch, PR mot main, og noen andre må godkjenne før merge.

  ## Testing

  17 enhetstester (xUnit), alle grønne. Dekker alle kontrollerne.

  Vi har også testet appen manuelt:

  | Hva vi testet | Hva som skjedde |
  |---|---|
  | Behov/Ressurs med gyldig data | Fungerer, viser bekreftelsesside |
  | Behov/Ressurs med tomme/ugyldige felt | Får feilmelding, ikke krasj |
  | Redigere/slette behov eller ressurs | Fungerer |
  | Registrere hendelse på kart | Vises med markør og radius |
  | Tomme felt på kartskjema | Nettleseren stopper innsending selv |
  | Ukjent URL | Tom 404-side |


## Bruk av KI

Vi har brukt KI som et hjelpemiddel gjennom programmeringsprosessen. KI har blant annet blitt brukt til å forklare kode og feilmeldinger, finne og rette feil, foreslå løsninger og hjelp med implementering av funksjonalitet.
KI har blitt brukt som et støtteverktøy, mens vi selv har vurdert, tilpasset og implementert løsningene i prosjektet.
