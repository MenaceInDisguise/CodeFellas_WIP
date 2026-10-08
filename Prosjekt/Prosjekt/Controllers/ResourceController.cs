using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;
using Prosjekt.Models.ModelView.ResourceType;
using System.Collections.Concurrent;
using System.Globalization;

namespace Prosjekt.Controllers
{
    /// <summary>
    /// Handles registering, displaying, editing and deleting resources.
    /// Each resource has a name, description, quantity and a position selected on the map (Leaflet).
    /// </summary>
    public class ResourceController : Controller
    {
        // Temporary in-memory storage instead of a real database.
        // The field is static so the data is shared between all requests (a new controller
        // is created per request), but everything disappears when the application is restarted.
        // ConcurrentDictionary is used because multiple requests can read/write at the same time.
        // The key is the resource's name, and the comparison ignores upper/lower case,
        // so "Water" and "water" are treated as the same resource.
        private static readonly ConcurrentDictionary<string, ResourceViewModel> _resourceDatabase = new(StringComparer.OrdinalIgnoreCase);

        // Shows an empty form for registering a new resource.
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ResourceCreatorViewModel());
        }

        // Receives the form from Index. On error the form is shown again with the user's
        // filled-in values; on success the resource is saved and a confirmation page (the Create view) is shown.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ResourceCreatorViewModel model)
        {
            // Check that a position is selected
            if (model.Latitude == 0.0 || model.Longitude == 0.0)
            {
                ModelState.AddModelError("", "You must select a position on the map.");
            }

            // Check that the list is not empty
            if (model.ResourceList == null || !model.ResourceList.Any())
            {
                ModelState.AddModelError("", "You must add at least one resource to the list.");
            }
            else
            {
                for (int i = 0; i < model.ResourceList.Count; i++)
                {
                    var item = model.ResourceList[i];
                    if (string.IsNullOrWhiteSpace(item.Name) || item.Quantity <= 0)
                    {
                        ModelState.AddModelError("", $"Resource #{i + 1} must have a valid name and a quantity above 0.");
                    }
                }
            }

            // Catches both the error above and any validation errors from model binding.
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            ResourceViewModel lastSavedResource = null;

            for (int i = 0; i < model.ResourceList.Count; i++)
            {
                var baseResource = model.ResourceList[i];
                baseResource.Latitude = model.Latitude.ToString(CultureInfo.InvariantCulture);
                baseResource.Longitude = model.Longitude.ToString(CultureInfo.InvariantCulture);

                ResourceViewModel resourceToSave = baseResource;

                switch (baseResource.Category)
                {
                    // Build the correct subclass based on the selected category
                    case ResourceViewModel.ResourceType.Vehicle:
                        string plate = Request.Form[$"ResourceList[{i}].LicensePlate"].ToString();
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
                            LicensePlate = plate,
                            Type = vehicleType
                        };
                        break;

                    case ResourceViewModel.ResourceType.Tool:
                        string toolTypeStr = Request.Form[$"ResourceList[{i}].ToolType"].ToString();
                        Enum.TryParse<ToolViewModel.ToolType>(toolTypeStr, out var toolType);
                        resourceToSave = new ToolViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            Type = toolType
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

                    case ResourceViewModel.ResourceType.Provisions:
                        string provTypeStr = Request.Form[$"ResourceList[{i}].ProvisionsType"].ToString();
                        Enum.TryParse<ProvisionsViewModel.ProvisionsType>(provTypeStr, out var provType);
                        resourceToSave = new ProvisionsViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            Type = provType
                        };
                        break;

                    case ResourceViewModel.ResourceType.Materials:
                        string matTypeStr = Request.Form[$"ResourceList[{i}].MaterialType"].ToString();
                        Enum.TryParse<MaterialsViewModel.MaterialType>(matTypeStr, out var matType);
                        resourceToSave = new MaterialsViewModel
                        {
                            Name = baseResource.Name,
                            Description = baseResource.Description,
                            Quantity = baseResource.Quantity,
                            Latitude = baseResource.Latitude,
                            Longitude = baseResource.Longitude,
                            Category = baseResource.Category,
                            Type = matType
                        };
                        break;
                }

                _resourceDatabase[resourceToSave.Name] = resourceToSave;

                // Save a reference to this one so we can show it on the receipt page
                lastSavedResource = resourceToSave;
            }

            // Return the Create view (the receipt) instead of Overview
            return View("Create", lastSavedResource);
        }

        // Checks that the coordinates are valid numbers and within allowed values
        // (latitude -90 to 90, longitude -180 to 180).
        // InvariantCulture is used because the map sends numbers with a period as the decimal separator (e.g. "59.91"),
        // whereas Norwegian culture would expect a comma and thus fail the parsing.
        private static bool IsValidPosition(string latitude, string longitude)
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

        // Shows a list of all registered resources.
        [HttpGet]
        public IActionResult Overview()
        {
            var allResources = _resourceDatabase.Values.ToList();
            return View(allResources);
        }

        // Shows the edit form for the resource with the given name.
        // The name works as the ID, since it is the key in the storage.
        [HttpGet]
        public IActionResult Edit(string name)
        {
            if (string.IsNullOrEmpty(name) || !_resourceDatabase.TryGetValue(name, out var resource))
            {
                return NotFound();
            }

            return View(resource);
        }

        // Saves changes to a resource.
        // originalName is sent along from the form (hidden field) so we know which resource
        // is being edited, even if the user has changed the name itself.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string originalName, ResourceViewModel model)
        {
            // Same requirements for mandatory fields as when creating.
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Description) || model.Quantity <= 0)
            {
                return View(model);
            }

            // Without the original name we don't know which resource to update.
            if (string.IsNullOrEmpty(originalName))
            {
                return BadRequest();
            }

            // The name is the key in the storage. If the name has changed, the old entry must be removed,
            // otherwise we would end up with two resources (old and new name).
            // NOTE: this happens before the position is validated below. If the validation fails,
            // the old entry has already been deleted without the new one being saved.
            if (!originalName.Equals(model.Name, StringComparison.OrdinalIgnoreCase))
            {
                _resourceDatabase.TryRemove(originalName, out _);
            }

            //Validates coordinates after the change
            if (!IsValidPosition(model.Latitude, model.Longitude))
            {
                ModelState.AddModelError("", "You must select a valid position.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ResourceViewModel resourceToSave = model;

            // Build the correct subclass based on the selected category
            switch (model.Category)
            {
                case ResourceViewModel.ResourceType.Vehicle:
                    string plate = Request.Form["LicensePlate"].ToString();
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
                        LicensePlate = plate,
                        Type = vehicleType
                    };
                    break;

                case ResourceViewModel.ResourceType.Tool:
                    string serial = Request.Form["SerialNumber"].ToString();
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

                case ResourceViewModel.ResourceType.Provisions:
                    string provTypeStr = Request.Form["ProvisionsType"].ToString();
                    Enum.TryParse<ProvisionsViewModel.ProvisionsType>(provTypeStr, out var provType);
                    resourceToSave = new ProvisionsViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                        Type = provType
                    };
                    break;

                case ResourceViewModel.ResourceType.Materials:
                    string matTypeStr = Request.Form["MaterialType"].ToString();
                    Enum.TryParse<MaterialsViewModel.MaterialType>(matTypeStr, out var matType);
                    resourceToSave = new MaterialsViewModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Category = model.Category,
                        Type = matType
                    };
                    break;
            }

            // Saves under the (possibly new) name. If the name already exists, that resource is overwritten.
_resourceDatabase[resourceToSave.Name] = resourceToSave;

            return RedirectToAction("Overview");
        }

        // Deletes the resource with the given name. POST only (with anti-forgery token), so that a resource
        // cannot be deleted by a regular link click. If the name doesn't exist, nothing happens.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _resourceDatabase.TryRemove(name, out _);
            }

            return RedirectToAction("Overview");
        }


    }
}
