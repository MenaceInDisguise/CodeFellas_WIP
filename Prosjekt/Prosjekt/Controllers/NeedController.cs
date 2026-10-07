using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using System.Globalization;
using System.Collections.Concurrent;

namespace Prosjekt.Controllers
{
    //Controller for registering and displaying needs.
    public class NeedController : Controller
    {
        private static readonly ConcurrentDictionary<string, NeedViewModel> _needDatabase = new(StringComparer.OrdinalIgnoreCase);
        //Shows the form for registering a new need.
        public IActionResult Index()
        {
            return View(new NeedViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(NeedViewModel model)
        {
            // Validates whether all required fields are filled in and whether the position is valid.
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Description) || model.Total <= 0)
            {
                ModelState.AddModelError("", "All fields must be filled in correctly.");
                return View("Index", model);
            }
            if (!IsValidPosition(model.Latitude, model.Longitude))
            {
                ModelState.AddModelError("", "You must select a position on the map.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            _needDatabase[model.Name] = model;

            return View(model);
        }
        [HttpGet]
        public IActionResult Overview()
        {
            var allNeeds = _needDatabase.Values.ToList();
            return View(allNeeds);
        }
        private static bool IsValidPosition(string latitude, string longitude)
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

        [HttpGet]
        public IActionResult Edit(string name)
        {
            if (string.IsNullOrEmpty(name) || !_needDatabase.TryGetValue(name, out var need))
            {
                return NotFound();
            }

            return View(need);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string originalName, NeedViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Description) || model.Total <= 0)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(originalName))
            {
                return BadRequest();
            }

            if (!originalName.Equals(model.Name, StringComparison.OrdinalIgnoreCase))
            {
                _needDatabase.TryRemove(originalName, out _);
            }

            //Validates coordinates after the change
            if (!IsValidPosition(model.Latitude, model.Longitude))
            {
                ModelState.AddModelError("", "You must select a valid position.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _needDatabase[model.Name] = model;

            return RedirectToAction("Overview");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _needDatabase.TryRemove(name, out _);
            }

            return RedirectToAction("Overview");
        }
    }
}
