using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Xunit;

namespace Prosjekt.Xunit
{
    public class CrisisInformationControllerTest
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var controller = new CrisisInformationController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}