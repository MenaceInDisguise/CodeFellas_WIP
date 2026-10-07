using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.Entities;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.ModelView.RessursType;
using System.Collections.Concurrent;

namespace Prosjekt.Controllers
{
    /// <summary>
    /// Håndterer registrering, visning, redigering og sletting av ressurser.
    /// Hver ressurs har navn, beskrivelse, antall og en posisjon valgt i kartet (Leaflet).
    /// </summary>
    public class RessursController : Controller
    {
        private static readonly ConcurrentDictionary<string, RessursViewModel> _ressursDatabase = new(StringComparer.OrdinalIgnoreCase);

        // Viser et tomt skjema for å registrere en ny ressurs.
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RessursOppretterViewModel());
        }

        // Tar imot skjemaet fra Index.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RessursOppretterViewModel model)
        {
            // Sjekk at posisjon er valgt og gyldig via Coordinates
            if (!model.Latitude.HasValue || !model.Longitude.HasValue)
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
            }
            else
            {
                var coords = new Coordinates(model.Latitude.Value, model.Longitude.Value);
                if (!coords.IsValid())
                {
                    ModelState.AddModelError("", "De oppgitte koordinatene er ugyldige.");
                }
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

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            RessursViewModel? sistLagredeRessurs = null;

            for (int i = 0; i < model.RessursListe.Count; i++)
            {
                var baseressurs = model.RessursListe[i];
                baseressurs.Latitude = model.Latitude;
                baseressurs.Longitude = model.Longitude;

                RessursViewModel ressursSomSkalLagres = baseressurs;

                switch (baseressurs.Kategori)
                {
                    case RessursViewModel.RessursType.Kjøretøy:
                        string skilt = Request.Form[$"RessursListe[{i}].Skiltnummer"].ToString();
                        string kjøretøyTypeStr = Request.Form[$"RessursListe[{i}].KjøretøyType"].ToString();
                        Enum.TryParse<KjøretøyViewModel.KjøretøyType>(kjøretøyTypeStr, out var kjøretøyType);
                        ressursSomSkalLagres = new KjøretøyViewModel
                        {
                            Navn = baseressurs.Navn,
                            Beskrivelse = baseressurs.Beskrivelse,
                            Antall = baseressurs.Antall,
                            Latitude = baseressurs.Latitude,
                            Longitude = baseressurs.Longitude,
                            Kategori = baseressurs.Kategori,
                            Skiltnummer = skilt,
                            Type = kjøretøyType
                        };
                        break;

                    case RessursViewModel.RessursType.Verktøy:
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
                sistLagredeRessurs = ressursSomSkalLagres;
            }

            return RedirectToAction(nameof(Oversikt));
        }

        // Viser en liste over alle registrerte ressurser.
        [HttpGet]
        public IActionResult Oversikt()
        {
            var alleRessurser = _ressursDatabase.Values.ToList();
            return View(alleRessurser);
        }

        // Viser redigeringsskjemaet for ressursen med gitt navn.
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string opprinneligNavn, RessursViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Antall <= 0)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(opprinneligNavn))
            {
                return BadRequest();
            }

            if (!model.Latitude.HasValue || !model.Longitude.HasValue)
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
                return View(model);
            }

            var coords = new Coordinates(model.Latitude.Value, model.Longitude.Value);
            if (!coords.IsValid())
            {
                ModelState.AddModelError("", "Du må velge en gyldig posisjon.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!opprinneligNavn.Equals(model.Navn, StringComparison.OrdinalIgnoreCase))
            {
                _ressursDatabase.TryRemove(opprinneligNavn, out _);
            }

            RessursViewModel ressursSomSkalLagres = model;

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

            _ressursDatabase[ressursSomSkalLagres.Navn] = ressursSomSkalLagres;

            return RedirectToAction(nameof(Oversikt));
        }

        // Sletter ressursen med gitt navn.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string navn)
        {
            if (!string.IsNullOrEmpty(navn))
            {
                _ressursDatabase.TryRemove(navn, out _);
            }

            return RedirectToAction(nameof(Oversikt));
        }
    }
}