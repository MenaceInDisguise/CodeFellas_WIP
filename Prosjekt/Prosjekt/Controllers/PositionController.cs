using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class PositionController : Controller
    {
        private static List<PositionModel> positions = new List<PositionModel>();

        [HttpGet]
        public IActionResult CorrectMap()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CorrectMap(PositionModel model)
        {
            if (ModelState.IsValid)
            {
                positions.Add(model);
                return View("CorrectionOverview", positions);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CorrectionOverview()
        {
            return View(positions);
        }

    }
}
