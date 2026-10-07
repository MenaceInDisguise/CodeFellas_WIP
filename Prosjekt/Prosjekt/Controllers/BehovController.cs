using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.Entities;
using System.Collections.Concurrent;

namespace Prosjekt.Controllers
{
    // Controller for registrering og visning av behov.
    public class BehovController : Controller
    {
        private static readonly ConcurrentDictionary<string, BehovViewModel> _behovDatabase = new(StringComparer.OrdinalIgnoreCase);

        // Viser skjemaet for å registrere et nytt behov.
        [HttpGet]
        public IActionResult Index()
        {
            return View(new BehovViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BehovViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Totalt <= 0)
            {
                ModelState.AddModelError("", "Alle felt må fylles ut gyldig.");
                return View("Index", model);
            }

            if (!model.Latitude.HasValue || !model.Longitude.HasValue)
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
                return View("Index", model);
            }

            var coords = new Coordinates(model.Latitude.Value, model.Longitude.Value);

            if (!coords.IsValid())
            {
                ModelState.AddModelError("", "De oppgitte koordinatene er ugyldige.");
                return View("Index", model);
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            _behovDatabase[model.Navn] = model;

            return RedirectToAction(nameof(Oversikt));
        }

        [HttpGet]
        public IActionResult Oversikt()
        {
            var alleBehov = _behovDatabase.Values.ToList();
            return View(alleBehov);
        }

        [HttpGet]
        public IActionResult Edit(string navn)
        {
            if (string.IsNullOrEmpty(navn) || !_behovDatabase.TryGetValue(navn, out var behov))
            {
                return NotFound();
            }

            return View(behov);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string opprinneligNavn, BehovViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Totalt <= 0)
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
                ModelState.AddModelError("", "De oppgitte koordinatene er ugyldige.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!opprinneligNavn.Equals(model.Navn, StringComparison.OrdinalIgnoreCase))
            {
                _behovDatabase.TryRemove(opprinneligNavn, out _);
            }

            _behovDatabase[model.Navn] = model;

            return RedirectToAction(nameof(Oversikt));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string navn)
        {
            if (!string.IsNullOrEmpty(navn))
            {
                _behovDatabase.TryRemove(navn, out _);
            }

            return RedirectToAction(nameof(Oversikt));
        }
    }
}