using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class AccountController : Controller
    {
        // Viser innloggingssiden (GET: /Account/Login)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Håndterer innlogging når knappen trykkes (POST: /Account/Login)
        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ViewBag.Error = "Vennligst oppgi et navn eller brukernavn.";
                return View();
            }

            // Lagrer valgt rolle og navn i sesjonen
            HttpContext.Session.SetString("UserRole", role ?? "Leverandor");
            HttpContext.Session.SetString("UserName", username);

            // Sender brukeren videre basert på valgt rolle
            if (role == "Offentlig")
            {
                // Offentlig aktør / kriseledelse sendes til full oversikt
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Privatperson / ressursleverandør sendes til ressursregistrering
                return RedirectToAction("Index", "Ressurs");
            }
        }

        // Logger ut ved å tømme sesjonen
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}