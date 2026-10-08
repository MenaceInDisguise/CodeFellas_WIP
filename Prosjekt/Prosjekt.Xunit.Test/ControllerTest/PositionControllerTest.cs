using Microsoft.AspNetCore.Mvc;
using Prosjekt.DataAccess.Repositories;
using Prosjekt.Controllers;
using Prosjekt.Models.Entities;
using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit
{
    public class PositionControllerTest
    {
        private sealed class FakeGeoChangeRepository : IGeoChangeRepository
        {
            private readonly List<GeoChange> _items = new();

            public Task AddAsync(GeoChange geoChange)
            {
                _items.Add(geoChange);
                return Task.CompletedTask;
            }

            public Task<IEnumerable<GeoChange>> GetAllAsync()
            {
                return Task.FromResult<IEnumerable<GeoChange>>(_items);
            }
        }

        [Fact]
        public void CorrectMap_Get_ReturnsViewResult()
        {
            var controller = new GeoChangeController(new FakeGeoChangeRepository());

            var result = controller.CorrectMap();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CorrectMap_Post_InvalidModelState_ReturnsViewWithModel()
        {
            var controller = new GeoChangeController(new FakeGeoChangeRepository());

            var model = new GeoChangeViewModel();

            controller.ModelState.AddModelError("Latitude", "Required");

            var result = await controller.CorrectMap(model);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Same(model, viewResult.Model);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task CorrectMap_Post_ValidModel_ReturnsRedirectToCorrectionOverview()
        {
            var controller = new GeoChangeController(new FakeGeoChangeRepository());

            var model = new GeoChangeViewModel
            {
                Latitude = 59.9,
                Longitude = 10.7,
                Description = "Test"
            };

            var result = await controller.CorrectMap(model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(GeoChangeController.CorrectionOverview), redirectResult.ActionName);
        }

        [Fact]
        public async Task CorrectionOverview_ReturnsViewResultWithPositions()
        {
            var controller = new GeoChangeController(new FakeGeoChangeRepository());

            var model = new GeoChangeViewModel
            {
                Latitude = 59.9,
                Longitude = 10.7,
                Description = "Test"
            };

            await controller.CorrectMap(model);

            var result = await controller.CorrectionOverview();

            var viewResult = Assert.IsType<ViewResult>(result);

            var models = Assert.IsType<List<GeoChangeViewModel>>(viewResult.Model);
            Assert.Contains(models, item =>
                item.Latitude == model.Latitude &&
                item.Longitude == model.Longitude &&
                item.Description == model.Description);
        }
    }
}