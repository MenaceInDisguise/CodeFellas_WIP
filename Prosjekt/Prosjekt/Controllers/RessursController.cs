using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using System.Collections.Concurrent;
using System.Globalization;

namespace Prosjekt.Controllers
{
    /// <summary>
    /// Håndterer registrering, visning, redigering og sletting av ressurser.
    /// Hver ressurs har navn, beskrivelse, antall og en posisjon valgt i kartet (Leaflet).
    /// </summary>
    public class RessursController : Controller
    {
        // Midlertidig lagring i minnet i stedet for en ekte database.
        // Feltet er static slik at dataene deles mellom alle forespørsler (en ny controller
        // opprettes per forespørsel), men alt forsvinner når applikasjonen startes på nytt.
        // ConcurrentDictionary brukes fordi flere forespørsler kan lese/skrive samtidig.
        // Nøkkelen er ressursens navn, og sammenligningen ignorerer store/små bokstaver,
        // så "Vann" og "vann" regnes som samme ressurs.
        private static readonly ConcurrentDictionary<string, RessursViewModel> _ressursDatabase = new(StringComparer.OrdinalIgnoreCase);

        // Viser et tomt skjema for å registrere en ny ressurs.
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RessursViewModel());
        }

        // Tar imot skjemaet fra Index. Ved feil vises skjemaet på nytt med brukerens
        // utfylte verdier; ved suksess lagres ressursen og en bekreftelsesside (Create-viewet) vises.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RessursViewModel model)
        {
            // Obligatoriske felt: navn, beskrivelse og et positivt antall.
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Antall <= 0)
            {
                ModelState.AddModelError("", "Alle felt må fylles ut gyldig.");
                return View("Index", model);
            }

            // Latitude/Longitude fylles inn av kartet i viewet når brukeren klikker på en posisjon.
            // Tomme verdier betyr at brukeren ikke har valgt noe punkt.
            // Merk: her sjekkes bare at feltene finnes, ikke at verdiene er gyldige (se ErGyldigPosisjon).
            if (string.IsNullOrWhiteSpace(model.Latitude) || string.IsNullOrWhiteSpace(model.Longitude))
            {
                ModelState.AddModelError("", "Du må velge en posisjon i kartet.");
            }

            // Fanger både feilen over og eventuelle valideringsfeil fra modellbindingen.
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Lagrer ressursen. Finnes navnet fra før, blir den eksisterende ressursen overskrevet.
            _ressursDatabase[model.Navn] = model;

            return View(model);
        }

        // Sjekker at koordinatene er gyldige tall og innenfor lovlige verdier
        // (breddegrad -90 til 90, lengdegrad -180 til 180).
        // InvariantCulture brukes fordi kartet sender tall med punktum som desimaltegn (f.eks. "59.91"),
        // mens norsk kultur ville forventet komma og dermed feilet parsingen.
        private static bool ErGyldigPosisjon(string latitude, string longitude)
        {
            return decimal.TryParse(
                       latitude,
                       NumberStyles.Float,
                       CultureInfo.InvariantCulture,
                       out var lat) &&
                   decimal.TryParse(
                       longitude,
                       NumberStyles.Float,
                       CultureInfo.InvariantCulture,
                       out var lon) &&
                   lat >= -90 &&
                   lat <= 90 &&
                   lon >= -180 &&
                   lon <= 180;
        }

        // Viser en liste over alle registrerte ressurser.
        [HttpGet]
        public IActionResult Oversikt()
        {
            var alleRessurser = _ressursDatabase.Values.ToList();
            return View(alleRessurser);
        }

        // Viser redigeringsskjemaet for ressursen med gitt navn.
        // Navnet fungerer som ID, siden det er nøkkelen i lagringen.
        [HttpGet]
        public IActionResult Edit(string navn)
        {
            if (string.IsNullOrEmpty(navn) || !_ressursDatabase.TryGetValue(navn, out var ressurs))
            {
                return NotFound();
            }

            return View(ressurs);
        }

        // Lagrer endringer på en ressurs.
        // opprinneligNavn sendes med fra skjemaet (skjult felt) slik at vi vet hvilken ressurs
        // som redigeres, selv om brukeren har endret selve navnet.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string opprinneligNavn, RessursViewModel model)
        {
            // Samme krav til obligatoriske felt som ved opprettelse.
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Antall <= 0)
            {
                return View(model);
            }

            // Uten opprinnelig navn vet vi ikke hvilken ressurs som skal oppdateres.
            if (string.IsNullOrEmpty(opprinneligNavn))
            {
                return BadRequest();
            }

            // Navnet er nøkkelen i lagringen. Er navnet endret, må den gamle oppføringen fjernes,
            // ellers ville vi fått to ressurser (gammelt og nytt navn).
            // OBS: dette skjer før posisjonen valideres under. Feiler valideringen,
            // er den gamle oppføringen allerede slettet uten at den nye er lagret.
            if (!opprinneligNavn.Equals(model.Navn, StringComparison.OrdinalIgnoreCase))
            {
                _ressursDatabase.TryRemove(opprinneligNavn, out _);
            }

            //Validerer koordinater etter endring
            if (!ErGyldigPosisjon(model.Latitude, model.Longitude))
            {
                ModelState.AddModelError("", "Du må velge en gyldig posisjon.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lagrer under (eventuelt nytt) navn. Finnes navnet fra før, blir den ressursen overskrevet.
            _ressursDatabase[model.Navn] = model;

            return RedirectToAction("Oversikt");
        }

        // Sletter ressursen med gitt navn. Kun POST (med anti-forgery-token), slik at en ressurs
        // ikke kan slettes ved et vanlig lenkeklikk. Finnes ikke navnet, skjer ingenting.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string navn)
        {
            if (!string.IsNullOrEmpty(navn))
            {
                _ressursDatabase.TryRemove(navn, out _);
            }

            return RedirectToAction("Oversikt");
        }


    }
}
