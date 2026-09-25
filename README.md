# Prosjekt Kartverket/Heimevernet
  Prosjekt/løsning laget av gruppe 12/15

  ### Gruppen består av:
  Aryan - Emil - Andreas - Bjørn Tore - Sigve - Leart

  ## Systemarkitektur

  Appen er laget i ASP.NET Core MVC (.NET 10) og kjøres via .NET Aspire.

  Kontrollere:
  - Home (forside, personvern, feilside)
  - Behov (opprette/redigere/slette behov, med viktighet og posisjon i kart)
  - Ressurs (opprette/redigere/slette ressurser, med posisjon i kart)
  - Position (registrere geografiske hendelser med kart og radius)
  - KriseInformasjon og Innstillinger (foreløpig bare placeholder-sider)

  Kart er laget med Leaflet. Klikker du i kartet fylles koordinatene ut automatisk.

  Vi har ikke noen ordentlig database koblet til ennå. Det er satt opp en MySqlConnection mot MariaDB i Program.cs (kjører via docker-compose), men ingen av kontrollerne bruker den foreløpig. Behov og
  ressurser lagres bare i minnet mens appen kjører, og forsvinner når den restartes.

  ## Drift

  Kjøre lokalt:
  ```
  dotnet run --project Prosjekt/Prosjekt
  ```

  Vi har også en docker-compose som kobler appen mot en MariaDB-container, men den må kjøre på et eget nettverk (appnet) som må settes opp separat.

  Ting som mangler:
  - Databasen er satt opp men ikke brukt
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
