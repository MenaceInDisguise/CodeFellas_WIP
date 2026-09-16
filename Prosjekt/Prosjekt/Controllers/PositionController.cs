using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class PositionController : Controller
    {
        //Låser tilgang til listen slik at flere forespørsler ikke
        //kan endre listen samtidig.
        private static readonly object PositionsLock = new();
        
        //Midlertidig lagring av posisjonene som er sendt inn.
        private static readonly List<PositionViewModel> positions = new();

        [HttpGet]
        //Viser kartet der brukeren kan registrere en posisjon.
        public IActionResult CorrectMap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Mottar posisjonsdata fra kartet og lagrer den i listen.
        public IActionResult CorrectMap(PositionViewModel model)
        {
            //Sjekker at dataene som ble sendt inn er gyldige.
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<PositionViewModel> snapshot;
            //Låser listen mens den nye posisjonene legges til.
            lock (PositionsLock)
            {
                positions.Add(model);
                snapshot = positions.ToList();
            }

            //Sender de registrerte posisjonene til oversikssiden.
            return View("CorrectionOverview", snapshot);
        }

        [HttpGet]
        //Viser en oversikt over alle registrerte posisjoner.
        public IActionResult CorrectionOverview()
        {
            //Låser listen mens posisjonene hentes ut.
            lock (PositionsLock)
            {
                return View(positions.ToList());
            }
        }

    }
}
