using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit
{
    public class PositionControllerTest
    {
        [Fact]
        public void CorrectMap_Get_ReturnsViewResult()
        {
            var controller = new PositionController();

            var result = controller.CorrectMap();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CorrectMap_Post_InvalidModelState_ReturnsViewWithModel()
        {
            var controller = new PositionController();

            var model = new PositionViewModel();

            controller.ModelState.AddModelError("Latitude", "Required");

            var result = controller.CorrectMap(model);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Same(model, viewResult.Model);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void CorrectMap_Post_ValidModel_ReturnsCorrectionOverviewModel()
        {
            var controller = new PositionController();

            var model = new PositionViewModel
            {
                Latitude = "59.9",
                Longitude = "10.7",
                Description = "Test"
            };

            var result = controller.CorrectMap(model);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("CorrectionOverview", viewResult.ViewName);

            var models = Assert.IsType<List<PositionViewModel>>(viewResult.Model);
            Assert.Contains(model, models);
        }

        [Fact]
        public void CorrectionOverview_ReturnsViewResultWithPositions()
        {
            var controller = new PositionController();

            var model = new PositionViewModel
            {
                Latitude = "59.9",
                Longitude = "10.7",
                Description = "Test"
            };

            controller.CorrectMap(model);

            var result = controller.CorrectionOverview();

            var viewResult = Assert.IsType<ViewResult>(result);

            var models = Assert.IsType<List<PositionViewModel>>(viewResult.Model);
            Assert.Contains(model, models);
        }
    }
}