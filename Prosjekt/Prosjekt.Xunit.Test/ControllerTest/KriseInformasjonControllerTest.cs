using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Xunit;

namespace Prosjekt.Xunit
{
    public class KriseInformasjonControllerTest
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var controller = new KriseInformasjonController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}