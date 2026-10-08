using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.Entities;
using System.Collections.Concurrent;
namespace Prosjekt.Controllers
{
    /// <summary>
    /// Provides MVC actions for creating, viewing, editing, and deleting NeedViewModel instances, using an in-memory,
    /// thread-safe dictionary keyed by name (case-insensitive).
    /// </summary>
    /// <remarks>POST actions use anti-forgery validation. Inputs are validated for required fields, positive
    /// totals, and valid geographic coordinates; invalid submissions return the input view with model-state errors.
    /// Edit supports renaming by removing the original key when the name changes; Delete removes an entry if
    /// present.</remarks>
    public class NeedController : Controller
    {
        private static readonly ConcurrentDictionary<string, NeedViewModel> _needDatabase = new(StringComparer.OrdinalIgnoreCase);

        [HttpGet]
        public IActionResult Index()
        {
            return View(new NeedViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NeedViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Description) || model.Total <= 0)
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

            _needDatabase[model.Name] = model;

            return View("Create", model);
        }

        [HttpGet]
        public IActionResult Overview()
        {
            var allNeeds = _needDatabase.Values.ToList();
            return View(allNeeds);
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

            if (!originalName.Equals(model.Name, StringComparison.OrdinalIgnoreCase))
            {
                _needDatabase.TryRemove(originalName, out _);
            }

            _needDatabase[model.Name] = model;

            return RedirectToAction(nameof(Overview));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _needDatabase.TryRemove(name, out _);
            }

            return RedirectToAction(nameof(Overview));
        }
    }
}