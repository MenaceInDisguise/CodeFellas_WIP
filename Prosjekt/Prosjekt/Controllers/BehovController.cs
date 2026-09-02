using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class BehovController : Controller
    {
        public IActionResult Index()
        {
            return View(new BehovViewModel());
        }
        [HttpPost]
        public ActionResult Create(BehovViewModel model)
        {   
            return View(model);
        }

    }
}
