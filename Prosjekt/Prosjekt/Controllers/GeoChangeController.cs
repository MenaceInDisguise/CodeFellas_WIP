using Microsoft.AspNetCore.Mvc;
using Prosjekt.DataAccess.Repositories;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.Entities;
namespace Prosjekt.Controllers
{
    public class GeoChangeController : Controller
    {
        /// <summary>
        /// Repository used to persist and retrieve geographic change records.
        /// </summary>
        /// <remarks>Assigned via constructor injection and accessed only within the declaring
        /// class.</remarks>
        private readonly IGeoChangeRepository _repo;

        public GeoChangeController(IGeoChangeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult CorrectMap()
        {
            var model = new GeoChangeViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CorrectMap(GeoChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ChangeTypes = new GeoChangeViewModel().ChangeTypes;
                return View(model);
            }

            if (!model.Latitude.HasValue || !model.Longitude.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Posisjon må velges i kartet.");
                model.ChangeTypes = new GeoChangeViewModel().ChangeTypes;
                return View(model);
            }

            var coords = new Coordinates(model.Latitude.Value, model.Longitude.Value);

            if (!coords.IsValid())
            {
                ModelState.AddModelError(string.Empty, "De oppgitte koordinatene er ugyldige.");
                model.ChangeTypes = new GeoChangeViewModel().ChangeTypes;
                return View(model);
            }

            var geoChange = new GeoChange
            {
                Coords = coords,
                Description = model.Description,
                Radius = model.Radius,
                ChangeTypes = model.SelectedChangeTypes != null && model.SelectedChangeTypes.Any()
                    ? string.Join(", ", model.SelectedChangeTypes)
                    : string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(geoChange);

            return RedirectToAction(nameof(CorrectionOverview));
        }

        [HttpGet]
        public async Task<IActionResult> CorrectionOverview()
        {
            var entities = await _repo.GetAllAsync();

            var viewModels = entities.Select(e => new GeoChangeViewModel
            {
                Latitude = e.Coords.Latitude,
                Longitude = e.Coords.Longitude,
                Description = e.Description,
                Radius = e.Radius,
                SelectedChangeTypes = !string.IsNullOrEmpty(e.ChangeTypes)
                    ? e.ChangeTypes.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>()
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public static List<GeoChangeViewModel> GetRegisteredPositions()
        {
            return new List<GeoChangeViewModel>();
        }
    }
}