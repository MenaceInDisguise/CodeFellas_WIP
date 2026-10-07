using Microsoft.AspNetCore.Mvc;
using Prosjekt.DataAccess.Repositories;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.Entities;

namespace Prosjekt.Controllers
{
    public class GeoEndringController : Controller
    {
        private readonly IGeoEndringRepository _repo;

        public GeoEndringController(IGeoEndringRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult CorrectMap()
        {
            var model = new GeoEndringViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CorrectMap(GeoEndringViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ChangeTypes = new GeoEndringViewModel().ChangeTypes;
                return View(model);
            }

            // 1. Sjekk at koordinatene faktisk er oppgitt (håndterer double?)
            if (!model.Latitude.HasValue || !model.Longitude.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Posisjon må velges i kartet.");
                model.ChangeTypes = new GeoEndringViewModel().ChangeTypes;
                return View(model);
            }

            // 2. Opprett koordinatobjektet med faktiske double-verdier (.Value)
            var coords = new Coordinates(model.Latitude.Value, model.Longitude.Value);

            // 3. Valider koordinatene via metoden i record struct-en
            if (!coords.IsValid())
            {
                ModelState.AddModelError(string.Empty, "De oppgitte koordinatene er ugyldige.");
                model.ChangeTypes = new GeoEndringViewModel().ChangeTypes;
                return View(model);
            }

            // 4. Tilordne koordinatobjektet til entiteten
            var geoEndring = new GeoEndring
            {
                Coords = coords,
                Description = model.Description,
                Radius = model.Radius,
                ChangeTypes = model.SelectedChangeTypes != null && model.SelectedChangeTypes.Any()
                    ? string.Join(", ", model.SelectedChangeTypes)
                    : string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(geoEndring);

            return RedirectToAction(nameof(CorrectionOverview));
        }

        [HttpGet]
        public async Task<IActionResult> CorrectionOverview()
        {
            var entities = await _repo.GetAllAsync();

            // 4. Hent ut bredde- og lengdegrad via e.Coords
            var viewModels = entities.Select(e => new GeoEndringViewModel
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
        public static List<GeoEndringViewModel> GetRegisteredPositions()
        {
            return new List<GeoEndringViewModel>();
        }
    }
}