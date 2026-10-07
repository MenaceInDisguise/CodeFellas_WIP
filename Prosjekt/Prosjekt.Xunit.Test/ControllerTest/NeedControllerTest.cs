using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit
{
    public class NeedControllerTest
    {
        [Fact]
        public void Create_ValidModel_ReturnsViewResult()
        {
            var controller = new NeedController();

            var model = new NeedViewModel
            {
                Name = "Test",
                Description = "Test description",
                Total = 1
            };

            var result = controller.Create(model);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsViewResultWithModelError()
        {
            var controller = new NeedController();

            var model = new NeedViewModel
            {
                Name = "",
                Description = "Test description",
                Total = 1,
                Latitude = "58.146700",
                Longitude = "7.995600"
            };

            var result = controller.Create(model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal("Index", viewResult.ViewName);
        }
    }
}