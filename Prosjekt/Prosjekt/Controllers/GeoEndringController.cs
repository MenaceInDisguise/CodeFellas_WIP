using Microsoft.AspNetCore.Mvc;
using Prosjekt.DataAccess.Repositories; // Husk å inkludere denne!
using Prosjekt.Models;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.Entities;

namespace Prosjekt.Controllers
{
    public class GeoEndringController : Controller
    {
        // Dependency Injection av repositoryet ditt (erstatter den statiske listen)
        private readonly IGeoEndringRepository _repo;

        public GeoEndringController(IGeoEndringRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        // Viser kartet der brukeren kan registrere en posisjon.
        public IActionResult CorrectMap()
        {
            var model = new GeoEndringViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Mottar posisjonsdata fra kartet og lagrer den i databasen.
        public async Task<IActionResult> CorrectMap(GeoEndringViewModel model)
        {
            // Sjekker at dataene som ble sendt inn er gyldige.
            if (!ModelState.IsValid)
            {
                model.ChangeTypes = new GeoEndringViewModel().ChangeTypes;
                return View(model);
            }

            // Mapper data fra ViewModellen over til Databasemodellen (Entity-klassen)
            var geoEndring = new GeoEndring
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Description = model.Description,
                Radius = model.Radius,
                // Slår sammen de valgte avkrysningsboksene til én tekststreng for databasen
                ChangeTypes = model.SelectedChangeTypes != null && model.SelectedChangeTypes.Any()
                    ? string.Join(", ", model.SelectedChangeTypes)
                    : string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            // Lagrer til databasen via repositoryet
            await _repo.AddAsync(geoEndring);

            // Sender brukeren videre til oversiktssiden
            return RedirectToAction(nameof(CorrectionOverview));
        }

        [HttpGet]
        // Viser en oversikt over alle registrerte posisjoner fra databasen.
        public async Task<IActionResult> CorrectionOverview()
        {
            // Henter rådata fra databasen
            var entities = await _repo.GetAllAsync();

            // Mapper om fra database-entitet (GeoEndring) til ViewModel slik at visningen forstår det
            var viewModels = entities.Select(e => new GeoEndringViewModel
            {
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                Description = e.Description,
                Radius = e.Radius,
                // Deler opp tekststrengen fra databasen tilbake til en liste med endringstyper
                SelectedChangeTypes = !string.IsNullOrEmpty(e.ChangeTypes)
                    ? e.ChangeTypes.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>()
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public static List<GeoEndringViewModel> GetRegisteredPositions()
        {
            // Returnerer en tom liste for å tilfredsstille eventuelle gamle referanser,
            // ettersom data nå hentes via databasen.
            return new List<GeoEndringViewModel>();
        }
    }
}