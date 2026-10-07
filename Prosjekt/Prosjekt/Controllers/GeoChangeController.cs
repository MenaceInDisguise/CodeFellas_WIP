using Microsoft.AspNetCore.Mvc;
using Prosjekt.DataAccess.Repositories; // Remember to include this!
using Prosjekt.Models;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.Entities;

namespace Prosjekt.Controllers
{
    public class GeoChangeController : Controller
    {
        // Dependency Injection of your repository (replaces the static list)
        private readonly IGeoChangeRepository _repo;

        public GeoChangeController(IGeoChangeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        // Shows the map where the user can register a position.
        public IActionResult CorrectMap()
        {
            var model = new GeoChangeViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Receives position data from the map and saves it in the database.
        public async Task<IActionResult> CorrectMap(GeoChangeViewModel model)
        {
            // Checks that the submitted data is valid.
            if (!ModelState.IsValid)
            {
                model.ChangeTypes = new GeoChangeViewModel().ChangeTypes;
                return View(model);
            }

            // Maps data from the ViewModel over to the database model (Entity class)
            var geoChange = new GeoChange
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Description = model.Description,
                Radius = model.Radius,
                // Joins the selected checkboxes into a single text string for the database
                ChangeTypes = model.SelectedChangeTypes != null && model.SelectedChangeTypes.Any()
                    ? string.Join(", ", model.SelectedChangeTypes)
                    : string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            // Saves to the database via the repository
            await _repo.AddAsync(geoChange);

            // Sends the user on to the overview page
            return RedirectToAction(nameof(CorrectionOverview));
        }

        [HttpGet]
        // Shows an overview of all registered positions from the database.
        public async Task<IActionResult> CorrectionOverview()
        {
            // Fetches raw data from the database
            var entities = await _repo.GetAllAsync();

            // Maps from the database entity (GeoChange) to the ViewModel so the view understands it
            var viewModels = entities.Select(e => new GeoChangeViewModel
            {
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                Description = e.Description,
                Radius = e.Radius,
                // Splits the text string from the database back into a list of change types
                SelectedChangeTypes = !string.IsNullOrEmpty(e.ChangeTypes)
                    ? e.ChangeTypes.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>()
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public static List<GeoChangeViewModel> GetRegisteredPositions()
        {
            // Returns an empty list to satisfy any old references,
            // since data is now fetched via the database.
            return new List<GeoChangeViewModel>();
        }
    }
}