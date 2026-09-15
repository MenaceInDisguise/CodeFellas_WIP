using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers;

public class RessursHandlerController : Controller
{
    // Dette erstatter foreleserens database (_resourceRepository) med en midlertidig ordbok i minnet
    private static readonly ConcurrentDictionary<string, RessursViewModel> _ressursDatabase = new(StringComparer.OrdinalIgnoreCase);

    [HttpGet]
    public IActionResult Index()
    {
        // 1. Henter ut alle ressursene (tilsvarer forelesers _resourceRepository.GetAll())
        var all = _ressursDatabase.Values.ToList();

        // 2. Mapper ressursene til visningsmodeller (Nøyaktig likt foreleserens .Select-metode)
        var model = all.Select(r => new RessursViewModel
        {
            Navn = r.Navn,
            Beskrivelse = r.Beskrivelse,
            Antall = r.Antall
        }).ToList();

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(RessursViewModel model)
    {
        if (model.Navn == null || model.Beskrivelse == null || model.Antall <= 0)
        {
            ModelState.AddModelError("", "Navn, beskrivelse eller antall kan ikke være ugyldig.");
            return View(model);
        }

        // Lagrer i dictionaryen i stedet for repository.Create()
        _ressursDatabase[model.Navn] = model;

        var newModel = new RessursViewModel
        {
            Navn = model.Navn,
            Beskrivelse = model.Beskrivelse,
            Antall = model.Antall
        };
        return View(newModel);
    }

    [HttpGet]
    public ActionResult Edit(string navn)
    {
        // Tilsvarer foreleserens _resourceRepository.GetById(id)
        if (string.IsNullOrEmpty(navn) || !_ressursDatabase.TryGetValue(navn, out var ressurs))
        {
            return NotFound();
        }

        var model = new RessursViewModel
        {
            Navn = ressurs.Navn,
            Beskrivelse = ressurs.Beskrivelse,
            Antall = ressurs.Antall
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(string opprinneligNavn, RessursViewModel model)
    {
        if (model.Navn == null || model.Beskrivelse == null || model.Antall <= 0)
        {
            return View(model);
        }

        if (string.IsNullOrEmpty(opprinneligNavn))
        {
            return BadRequest();
        }

        // Hvis navnet har endret seg, fjerner vi den gamle oppføringen før vi legger inn ny
        if (opprinneligNavn != model.Navn)
        {
            _ressursDatabase.TryRemove(opprinneligNavn, out _);
        }

        // Oppdaterer i stedet for repository.Update()
        _ressursDatabase[model.Navn] = model;

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(string navn)
    {
        // Sletter fra dictionaryen i stedet for repository.Delete()
        if (string.IsNullOrEmpty(navn) || !_ressursDatabase.TryRemove(navn, out _))
        {
            return NotFound();
        }

        return RedirectToAction("Index");
    }
}