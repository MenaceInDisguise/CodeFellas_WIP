using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.ModelView.RessursType;
using System.Collections.Concurrent;
using System.Globalization;

namespace Prosjekt.Controllers
{
    /// <summary>
    /// Håndterer registrering, visning, redigering og sletting av ressurser.
    /// Hver ressurs har navn, beskrivelse, antall og en posisjon valgt i kartet (Leaflet).
    /// </summary>
    public class RessursController : Controller
    {
        // Midlertidig lagring i minnet i stedet for en ekte database.
        // Feltet er static slik at dataene deles mellom alle forespørsler (en ny controller
        // opprettes per forespørsel), men alt forsvinner når applikasjonen startes på nytt.
        // ConcurrentDictionary brukes fordi flere forespørsler kan lese/skrive samtidig.
        // Nøkkelen er ressursens navn, og sammenligningen ignorerer store/små bokstaver,
        // så "Vann" og "vann" regnes som samme ressurs.
        private static readonly ConcurrentDictionary<string, RessursViewModel> _ressursDatabase = new(StringComparer.OrdinalIgnoreCase);

        // Viser et tomt skjema for å registrere en ny ressurs.
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RessursOppretterViewModel());
        }

        // Tar imot skjemaet fra Index. Ved feil vises skjemaet på nytt med brukerens
        // utfylte verdier; ved suksess lagres ressursen og en bekreftelsesside (Create-viewet) vises.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RessursOppretterViewModel model)
        {
            // Sjekk at posisjon er valgt
            if (model.Latitude == 0.0 || model.Longitude == 0.0)
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
            }

            // Sjekk at listen ikke er tom
            if (model.RessursListe == null || !model.RessursListe.Any())
            {
                ModelState.AddModelError("", "Du må legge til minst én ressurs i listen.");
            }
            else
            {
                for (int i = 0; i < model.RessursListe.Count; i++)
                {
                    var item = model.RessursListe[i];
                    if (string.IsNullOrWhiteSpace(item.Navn) || item.Antall <= 0)
                    {
                        ModelState.AddModelError("", $"Ressurs #{i + 1} må ha et gyldig navn og antall over 0.");
                    }
                }
            }

            // Fanger både feilen over og eventuelle valideringsfeil fra modellbindingen.
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            for (int i = 0; i < model.RessursListe.Count; i++)
            {
                var baseressurs = model.RessursListe[i];
                baseressurs.Latitude = model.Latitude.ToString(CultureInfo.InvariantCulture);
                baseressurs.Longitude = model.Longitude.ToString(CultureInfo.InvariantCulture);

                RessursViewModel ressursSomSkalLagres = baseressurs;

                switch (baseressurs.Kategori)
                {
                    case RessursViewModel.RessursType.Kjøretøy:
                        string skilt = Request.Form[$"RessursListe[{i}].Skiltnummer"].ToString();
                        ressursSomSkalLagres = new KjøretøyViewModel
                        {
                            Navn = baseressurs.Navn,
                            Beskrivelse = baseressurs.Beskrivelse,
                            Antall = baseressurs.Antall,
                            Latitude = baseressurs.Latitude,
                            Longitude = baseressurs.Longitude,
                            Kategori = baseressurs.Kategori,
                            Skiltnummer = skilt
                        };
                        break;

                    case RessursViewModel.RessursType.Verktøy:
                        string serie = Request.Form[$"RessursListe[{i}].Serienummer"].ToString();
                        ressursSomSkalLagres = new VerktøyViewModel
                        {
                            Navn = baseressurs.Navn,
                            Beskrivelse = baseressurs.Beskrivelse,
                            Antall = baseressurs.Antall,
                            Latitude = baseressurs.Latitude,
                            Longitude = baseressurs.Longitude,
                            Kategori = baseressurs.Kategori,
                        };
                        break;

                    case RessursViewModel.RessursType.Klær:
                        string storrelseStr = Request.Form[$"RessursListe[{i}].Størrelse"].ToString();
                        Enum.TryParse<KlærViewModel.KlærStørrelse>(storrelseStr, out var str);
                        ressursSomSkalLagres = new KlærViewModel
                        {
                            Navn = baseressurs.Navn,
                            Beskrivelse = baseressurs.Beskrivelse,
                            Antall = baseressurs.Antall,
                            Latitude = baseressurs.Latitude,
                            Longitude = baseressurs.Longitude,
                            Kategori = baseressurs.Kategori,
                            Størrelse = str
                        };
                        break;

                    case RessursViewModel.RessursType.Provisjon:
                        string provTypeStr = Request.Form[$"RessursListe[{i}].ProvisjonType"].ToString();
                        Enum.TryParse<ProvisjonViewModel.Provisjonstype>(provTypeStr, out var provType);
                        ressursSomSkalLagres = new ProvisjonViewModel
                        {
                            Navn = baseressurs.Navn,
                            Beskrivelse = baseressurs.Beskrivelse,
                            Antall = baseressurs.Antall,
                            Latitude = baseressurs.Latitude,
                            Longitude = baseressurs.Longitude,
                            Kategori = baseressurs.Kategori,
                            Type = provType
                        };
                        break;

                    case RessursViewModel.RessursType.Materialer:
                        string matTypeStr = Request.Form[$"RessursListe[{i}].Materialtype"].ToString();
                        Enum.TryParse<MaterialerViewModel.MaterialerType>(matTypeStr, out var matType);
                        ressursSomSkalLagres = new MaterialerViewModel
                        {
                            Navn = baseressurs.Navn,
                            Beskrivelse = baseressurs.Beskrivelse,
                            Antall = baseressurs.Antall,
                            Latitude = baseressurs.Latitude,
                            Longitude = baseressurs.Longitude,
                            Kategori = baseressurs.Kategori,
                            Type = matType
                        };
                        break;
                }

                _ressursDatabase[ressursSomSkalLagres.Navn] = ressursSomSkalLagres;
            }

            return RedirectToAction("Oversikt");
        }

        // Sjekker at koordinatene er gyldige tall og innenfor lovlige verdier
        // (breddegrad -90 til 90, lengdegrad -180 til 180).
        // InvariantCulture brukes fordi kartet sender tall med punktum som desimaltegn (f.eks. "59.91"),
        // mens norsk kultur ville forventet komma og dermed feilet parsingen.
        private static bool ErGyldigPosisjon(string latitude, string longitude)
        {
            return decimal.TryParse(
                       latitude,
                       NumberStyles.Float,
                       CultureInfo.InvariantCulture,
                       out var lat) &&
                   decimal.TryParse(
                       longitude,
                       NumberStyles.Float,
                       CultureInfo.InvariantCulture,
                       out var lon) &&
                   lat >= -90 &&
                   lat <= 90 &&
                   lon >= -180 &&
                   lon <= 180;
        }

        // Viser en liste over alle registrerte ressurser.
        [HttpGet]
        public IActionResult Oversikt()
        {
            var alleRessurser = _ressursDatabase.Values.ToList();
            return View(alleRessurser);
        }

        // Viser redigeringsskjemaet for ressursen med gitt navn.
        // Navnet fungerer som ID, siden det er nøkkelen i lagringen.
        [HttpGet]
        public IActionResult Edit(string navn)
        {
            if (string.IsNullOrEmpty(navn) || !_ressursDatabase.TryGetValue(navn, out var ressurs))
            {
                return NotFound();
            }

            return View(ressurs);
        }

        // Lagrer endringer på en ressurs.
        // opprinneligNavn sendes med fra skjemaet (skjult felt) slik at vi vet hvilken ressurs
        // som redigeres, selv om brukeren har endret selve navnet.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string opprinneligNavn, RessursViewModel model)
        {
            // Samme krav til obligatoriske felt som ved opprettelse.
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Antall <= 0)
            {
                return View(model);
            }

            // Uten opprinnelig navn vet vi ikke hvilken ressurs som skal oppdateres.
            if (string.IsNullOrEmpty(opprinneligNavn))
            {
                return BadRequest();
            }

            // Navnet er nøkkelen i lagringen. Er navnet endret, må den gamle oppføringen fjernes,
            // ellers ville vi fått to ressurser (gammelt og nytt navn).
            // OBS: dette skjer før posisjonen valideres under. Feiler valideringen,
            // er den gamle oppføringen allerede slettet uten at den nye er lagret.
            if (!opprinneligNavn.Equals(model.Navn, StringComparison.OrdinalIgnoreCase))
            {
                _ressursDatabase.TryRemove(opprinneligNavn, out _);
            }

            //Validerer koordinater etter endring
            if (!ErGyldigPosisjon(model.Latitude, model.Longitude))
            {
                ModelState.AddModelError("", "Du må velge en gyldig posisjon.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RessursViewModel ressursSomSkalLagres = model;

            // Bygg riktig underklasse basert på valgt kategori
            switch (model.Kategori)
            {
                case RessursViewModel.RessursType.Kjøretøy:
                    string skilt = Request.Form["Skiltnummer"].ToString();
                    string kjøretøyTypeStr = Request.Form["KjøretøyType"].ToString();
                    Enum.TryParse<KjøretøyViewModel.KjøretøyType>(kjøretøyTypeStr, out var kjøretøyType);

                    ressursSomSkalLagres = new KjøretøyViewModel
                    {
                        Navn = model.Navn,
                        Beskrivelse = model.Beskrivelse,
                        Antall = model.Antall,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Kategori = model.Kategori,
                        Skiltnummer = skilt,
                        Type = kjøretøyType
                    };
                    break;

                case RessursViewModel.RessursType.Verktøy:
                    string serie = Request.Form["Serienummer"].ToString();
                    ressursSomSkalLagres = new VerktøyViewModel
                    {
                        Navn = model.Navn,
                        Beskrivelse = model.Beskrivelse,
                        Antall = model.Antall,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Kategori = model.Kategori,
                    };
                    break;

                case RessursViewModel.RessursType.Klær:
                    string storrelseStr = Request.Form["Størrelse"].ToString();
                    Enum.TryParse<KlærViewModel.KlærStørrelse>(storrelseStr, out var str);
                    ressursSomSkalLagres = new KlærViewModel
                    {
                        Navn = model.Navn,
                        Beskrivelse = model.Beskrivelse,
                        Antall = model.Antall,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Kategori = model.Kategori,
                        Størrelse = str
                    };
                    break;

                case RessursViewModel.RessursType.Provisjon:
                    string provTypeStr = Request.Form["ProvisjonType"].ToString();
                    Enum.TryParse<ProvisjonViewModel.Provisjonstype>(provTypeStr, out var provType);
                    ressursSomSkalLagres = new ProvisjonViewModel
                    {
                        Navn = model.Navn,
                        Beskrivelse = model.Beskrivelse,
                        Antall = model.Antall,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Kategori = model.Kategori,
                        Type = provType
                    };
                    break;

                case RessursViewModel.RessursType.Materialer:
                    string matTypeStr = Request.Form["Materialtype"].ToString();
                    Enum.TryParse<MaterialerViewModel.MaterialerType>(matTypeStr, out var matType);
                    ressursSomSkalLagres = new MaterialerViewModel
                    {
                        Navn = model.Navn,
                        Beskrivelse = model.Beskrivelse,
                        Antall = model.Antall,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Kategori = model.Kategori,
                        Type = matType
                    };
                    break;
            }

            // Lagrer under (eventuelt nytt) navn. Finnes navnet fra før, blir den ressursen overskrevet.
            _ressursDatabase[model.Navn] = model;

            return RedirectToAction("Oversikt");
        }

        // Sletter ressursen med gitt navn. Kun POST (med anti-forgery-token), slik at en ressurs
        // ikke kan slettes ved et vanlig lenkeklikk. Finnes ikke navnet, skjer ingenting.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string navn)
        {
            if (!string.IsNullOrEmpty(navn))
            {
                _ressursDatabase.TryRemove(navn, out _);
            }

            return RedirectToAction("Oversikt");
        }


    }
}
