using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using System.Globalization;
using System.Collections.Concurrent;

namespace Prosjekt.Controllers
{
    //Controller for registrering og visning av behov.
    public class BehovController : Controller
    {
        private static readonly ConcurrentDictionary<string, BehovViewModel> _behovDatabase = new(StringComparer.OrdinalIgnoreCase);
        //Viser skjemaet for å registrere et nytt behov.
        public IActionResult Index()
        {
            return View(new BehovViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(BehovViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Totalt <= 0)
            {
                ModelState.AddModelError("", "Alle felt må fylles ut gyldig.");
                return View("Index", model);
            }
            if (!ErGyldigPosisjon(model.Latitude, model.Longitude))
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            _behovDatabase[model.Navn] = model;

            return View(model);
        }
        [HttpGet]
        public IActionResult Oversikt()
        {
            var alleBehov = _behovDatabase.Values.ToList();
            return View(alleBehov);
        }
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
    }
}
