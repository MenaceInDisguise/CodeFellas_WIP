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
        public void Create_InvalidModel_ThrowsArgumentException()
        {
            var controller = new BehovController();

            var model = new BehovViewModel
            {
                Navn = "Test",
                Beskrivelse = "Testbeskrivelse",
                Totalt = 0,
            };

            Assert.Throws<ArgumentException>(() => controller.Create(model));
        }
    }
}