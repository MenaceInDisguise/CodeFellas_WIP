using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class PositionController : Controller
    {
        private static readonly object PositionsLock = new();
        private static readonly List<PositionModel> positions = new();

        [HttpGet]
        public IActionResult CorrectMap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CorrectMap(PositionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<PositionModel> snapshot;
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
