using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class RessursController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new Models.ModelView.RessursViewModel
            {
                Navn = "Traktor",
                Beskrivelse = "Traktor med henger, parkert på åker og enger",
                Antall = 1 // Siden din modell bruker et tall (int) i stedet for tekst ("Kjøretøy")
            };

            return View(viewModel);
        }
    }
}