using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Xunit;

namespace Prosjekt.Xunit;

public class HomeControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        var controller = new HomeController();
        var result = controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Personvern_ReturnsViewResult()
    {
        var controller = new HomeController();
        var result = controller.Personvern();
        Assert.IsType<ViewResult>(result);
    }
}
