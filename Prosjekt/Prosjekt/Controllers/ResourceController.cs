using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.Entities;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.ModelView.ResourceType;
using System.Collections.Concurrent;

namespace Prosjekt.Controllers
{
    /// <summary>
    /// Provides MVC actions to create, edit, list, and delete resources stored in an in-memory, thread-safe dictionary.
    /// </summary>
    /// <remarks>Validates model state and coordinates, maps posted form fields to specialized view models
    /// (Vehicle, Tool, Clothing, Provision, Materials), and protects POST actions with anti-forgery validation.
    /// Resources are keyed by name using a case-insensitive, concurrent dictionary as the in-memory store.</remarks>
    public class ResourceController : Controller
    {
        private static readonly ConcurrentDictionary<string, ResourceViewModel> _resourceDatabase = new(StringComparer.OrdinalIgnoreCase);

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ResourceCreatorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ResourceCreatorViewModel model)
        {
            if (model.Latitude.HasValue && model.Longitude.HasValue && model.ResourceList != null)
            {
                foreach (var item in model.ResourceList)
                {
                    item.Latitude = model.Latitude;
                    item.Longitude = model.Longitude;
                }

                foreach (var key in ModelState.Keys.Where(k => k.EndsWith(".Latitude") || k.EndsWith(".Longitude")).ToList())
                {
                    ModelState.Remove(key);
                }
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (model.ResourceList == null || !model.ResourceList.Any())
            {
                ModelState.AddModelError("", "Du må legge til minst én ressurs i listen.");
                return View("Index", model);
            }

            var coords = new Coordinates(model.Latitude!.Value, model.Longitude!.Value);
            if (!coords.IsValid())
            {
                ModelState.AddModelError("", "De oppgitte koordinatene er ugyldige.");
                return View("Index", model);
            }

            ResourceViewModel? lastSavedResource = null;

            for (int i = 0; i < model.ResourceList.Count; i++)
            {
                var baseResource = model.ResourceList[i];
                baseResource.Latitude = model.Latitude;
                baseResource.Longitude = model.Longitude;

                ResourceViewModel resourceToSave = baseResource;

                switch (baseResource.Category)
                {
                    case ResourceViewModel.ResourceType.Vehicle:
                        string licensePlate = Request.Form[$"ResourceList[{i}].LicensePlate"].ToString();
                        string vehicleTypeStr = Request.Form[$"ResourceList[{i}].VehicleType"].ToString();
                        Enum.TryParse<VehicleViewModel.VehicleType>(vehicleTypeStr, out var vehicleType);
                        resourceToSave = new VehicleViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            LicensePlate = licensePlate,
                            Type = vehicleType
                        };
                        break;

                    case ResourceViewModel.ResourceType.Tool:
                        resourceToSave = new ToolViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                        };
                        break;

                    case ResourceViewModel.ResourceType.Clothing:
                        string sizeStr = Request.Form[$"ResourceList[{i}].Size"].ToString();
                        Enum.TryParse<ClothingViewModel.ClothingSize>(sizeStr, out var size);
                        resourceToSave = new ClothingViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            Size = size
                        };
                        break;

                    case ResourceViewModel.ResourceType.Provision:
                        string provisionTypeStr = Request.Form[$"ResourceList[{i}].ProvisionType"].ToString();
                        Enum.TryParse<ProvisionViewModel.ProvisionType>(provisionTypeStr, out var provisionType);
                        resourceToSave = new ProvisionViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            Type = provisionType
                        };
                        break;

                    case ResourceViewModel.ResourceType.Materials:
                        string materialTypeStr = Request.Form[$"ResourceList[{i}].MaterialType"].ToString();
                        Enum.TryParse<MaterialsViewModel.MaterialType>(materialTypeStr, out var materialType);
                        resourceToSave = new MaterialsViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            Type = materialType
                        };
                        break;
                }

                _resourceDatabase[resourceToSave.Name] = resourceToSave;
                lastSavedResource = resourceToSave;
            }

            // Siden vi har bekreftet at ResourceList ikke er tom, vil lastSavedResource alltid ha en verdi her
            return View("Create", lastSavedResource);
        }

        [HttpGet]
        public IActionResult Overview()
        {
            var allResources = _resourceDatabase.Values.ToList();
            return View(allResources);
        }

        [HttpGet]
        public IActionResult Edit(string name)
        {
            if (string.IsNullOrEmpty(name) || !_resourceDatabase.TryGetValue(name, out var resource))
            {
                return NotFound();
            }

            return View(resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string originalName, ResourceViewModel model)
        {
            if (string.IsNullOrEmpty(originalName))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var coords = new Coordinates(model.Latitude!.Value, model.Longitude!.Value);
            if (!coords.IsValid())
            {
                ModelState.AddModelError("", "Du må velge en gyldig posisjon.");
                return View(model);
            }

            if (!originalName.Equals(model.Name, StringComparison.OrdinalIgnoreCase))
            {
                _resourceDatabase.TryRemove(originalName, out _);
            }

            ResourceViewModel resourceToSave = model;

            switch (model.Category)
            {
                case ResourceViewModel.ResourceType.Vehicle:
                    string licensePlate = Request.Form["LicensePlate"].ToString();
                    string vehicleTypeStr = Request.Form["VehicleType"].ToString();
                    Enum.TryParse<VehicleViewModel.VehicleType>(vehicleTypeStr, out var vehicleType);

                    resourceToSave = new VehicleViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                        LicensePlate = licensePlate,
                        Type = vehicleType
                    };
                    break;

                case ResourceViewModel.ResourceType.Tool:
                    resourceToSave = new ToolViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                    };
                    break;

                case ResourceViewModel.ResourceType.Clothing:
                    string sizeStr = Request.Form["Size"].ToString();
                    Enum.TryParse<ClothingViewModel.ClothingSize>(sizeStr, out var size);
                    resourceToSave = new ClothingViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                        Size = size
                    };
                    break;

                case ResourceViewModel.ResourceType.Provision:
                    string provisionTypeStr = Request.Form["ProvisionType"].ToString();
                    Enum.TryParse<ProvisionViewModel.ProvisionType>(provisionTypeStr, out var provisionType);
                    resourceToSave = new ProvisionViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                        Type = provisionType
                    };
                    break;

                case ResourceViewModel.ResourceType.Materials:
                    string materialTypeStr = Request.Form["MaterialType"].ToString();
                    Enum.TryParse<MaterialsViewModel.MaterialType>(materialTypeStr, out var materialType);
                    resourceToSave = new MaterialsViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                        Type = materialType
                    };
                    break;
            }

            _resourceDatabase[resourceToSave.Name] = resourceToSave;

            return RedirectToAction(nameof(Overview));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _resourceDatabase.TryRemove(name, out _);
            }

            return RedirectToAction(nameof(Overview));
        }
    }
}