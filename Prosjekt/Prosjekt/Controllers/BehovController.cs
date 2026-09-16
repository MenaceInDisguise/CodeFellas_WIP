using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    //Controller for registrering og visning av behov.
    public class BehovController : Controller
    {
        //Viser skjemaet for å registrere et nytt behov.
        public IActionResult Index()
        {
            return View(new BehovViewModel());
        }
        
        [HttpPost]
        public ActionResult Create(BehovViewModel model)
        {   
            //Kontrollerer at navn, beskrivelse og antall er gyldig.
            if (model.Navn == null || model.Beskrivelse == null || model.Totalt <= 0)
            {
                throw new ArgumentException("Navn, beskrivelse eller antall kan ikke være null.");
            }
            //Sender dataene videre til viewet for for visning.
            return View(model);
        }

    }
}
