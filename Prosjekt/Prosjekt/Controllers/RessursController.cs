using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class RessursController : Controller
    {
        private static readonly ConcurrentDictionary<string, RessursViewModel> _ressursDatabase = new(StringComparer.OrdinalIgnoreCase);

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RessursViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RessursViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Antall <= 0)
            {
                ModelState.AddModelError("", "Alle felt må fylles ut gyldig.");
                return View("Index", model);
            }

            _ressursDatabase[model.Navn] = model;

            if (string.IsNullOrWhiteSpace(model.Latitude) || string.IsNullOrWhiteSpace(model.Longitude))
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Oversikt()
        {
            var alleRessurser = _ressursDatabase.Values.ToList();
            return View(alleRessurser);
        }

        [HttpGet]
        public IActionResult Edit(string navn)
        {
            if (string.IsNullOrEmpty(navn) || !_ressursDatabase.TryGetValue(navn, out var ressurs))
            {
                return NotFound();
            }

            return View(ressurs);
        }

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

            if (!opprinneligNavn.Equals(model.Navn, StringComparison.OrdinalIgnoreCase))
            {
                _ressursDatabase.TryRemove(opprinneligNavn, out _);
            }

            _ressursDatabase[model.Navn] = model;

            return RedirectToAction("Oversikt");
        }

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