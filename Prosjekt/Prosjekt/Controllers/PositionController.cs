using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class PositionController : Controller
    {
        private static readonly object PositionsLock = new();
        private static readonly List<PositionViewModel> positions = new();

        [HttpGet]
        public IActionResult CorrectMap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CorrectMap(PositionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<PositionViewModel> snapshot;
            lock (PositionsLock)
            {
                positions.Add(model);
                snapshot = positions.ToList();
            }

            return View("CorrectionOverview", snapshot);
        }

        [HttpGet]
        public IActionResult CorrectionOverview()
        {
            lock (PositionsLock)
            {
                return View(positions.ToList());
            }
        }

    }
}
