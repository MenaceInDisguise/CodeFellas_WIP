# Prosjekt Kartverket/Heimevernet
Prosjekt/løsning laget av gruppe 12/15

### Gruppen består av:
Aryan - Emil - Andreas - Bjørn Tore - Sigve - Leart

 ## Systemarkitektur

  Løsningen er en **ASP.NET Core MVC**-applikasjon (.NET 10), orkestrert med **.NET Aspire**.

  | Controller | Views | Formål |
  |---|---|---|
  | `HomeController` | Index, Personvern, Error | Forside, personvernside og feilside |
  | `BehovController` | Index, Create | Registrere behov, med posisjon valgt i kart |
  | `RessursController` | Index, Oversikt, Edit, Delete | Full CRUD for ressurser, med posisjon i kart |
  | `PositionController` | CorrectMap, CorrectionOverview | Registrere geografiske hendelser med radius, vises på kart |
  | `KriseInformasjonController` | Index | Kriseinformasjon (placeholder) |
  | `InnstillingerController` | Index | Innstillinger (placeholder) |

  Kartfunksjonalitet er implementert med **Leaflet**: klikk i kartet fyller automatisk ut Breddegrad/Lengdegrad i skjemaene.

  **Datalagring:** Det finnes ingen database i aktiv bruk ennå. `Program.cs` registrerer en `MySqlConnection` mot MariaDB (koblingsstreng satt via `docker-compose.yml`), men ingen kontroller bruker den per nå.
  Data lagres i stedet i minnet.

  Alt forsvinner ved omstart av applikasjonen.

  ## Drift

  ### Kjøre lokalt
  ```
  dotnet run --project Prosjekt/Prosjekt
  ```
  eller via Aspire AppHost:
  ```
  dotnet run --project Prosjekt/Prosjekt.AppHost
  ```

  ### Docker / database
  `docker-compose.yml` og `docker-compose.override.yml` kobler appen til en MariaDB-container (`mariadbcontainer`) på et delt Docker-nettverk (`appnet`). Dette nettverket og containeren må finnes/opprettes
  separat før `docker compose up` kjøres.

  ### Nåværende status / mangler
  - Ingen CI/CD-pipeline satt opp
  - Database registrert i kode, men ikke koblet til noen kontroller
  - Ingen egendefinert 404-side

  ### Teamets workflow
  Hvert medlem jobber på egen branch, åpner PR mot `main`, og branch protection krever godkjenning fra et annet medlem før merge. Branch slettes etter merge.

  ## Testing — scenarier og resultater

  ### Enhetstester
  17 xUnit-tester i `Prosjekt.Xunit.Test`, kjørt med `dotnet test`:
  ```
  Passed!  - Failed: 0, Passed: 17, Skipped: 0, Total: 17
  ```
  Alle 6 kontrollere har testdekning.

  ### Funksjonelle testscenarier (manuell testing i nettleser)

  | Scenario | Resultat |
  |---|---|
  | Behov/Ressurs, gyldig data | Fungerer, viser bekreftelsesside |
  | Behov/Ressurs, ugyldig data | Viser feilmelding "Alle felt må fylles ut gyldig." |
  | Ressurs: rediger eksisterende | Fungerer |
  | Ressurs: slett | Fungerer (bruker nettleser-bekreftelsesdialog) |
  | Registrere geografisk hendelse, gyldig | Lagres, vises med markør og radius-sirkel på kartoversikt |
  | Registrere geografisk hendelse, tomme felt | Blokkeres av nettleserens innebygde feltvalidering |
  | Kriseinformasjon / Innstillinger / Personvern | Laster fint (placeholder-innhold) |
  | Ukjent URL | Tom 404-side |

  ### Kjente begrensninger
  - Database er forberedt i kode men ikke i bruk — all data er ikke-persistert
  - Slett-knapp bruker nettleserens `confirm()`, ikke egen bekreftelsesside
  - Ingen egendefinert "fant ikke siden"-side
