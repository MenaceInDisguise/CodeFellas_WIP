using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit
{
    public class BehovControllerTest
    {
        [Fact]
        public void Create_ValidModel_ReturnsViewResult()
        {
            var controller = new BehovController();

            var model = new BehovViewModel
            {
                Navn = "Test",
                Beskrivelse = "Testbeskrivelse",
                Totalt = 1
            };

            var result = controller.Create(model);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModel_ReturnsViewResultWithModelError()
        {
            var controller = new BehovController();

            var model = new BehovViewModel
            {
                Navn = "",
                Beskrivelse = "Testbeskrivelse",
                Totalt = 1
            };

            var result = controller.Create(model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal("Index", viewResult.ViewName);
        }
    }
}