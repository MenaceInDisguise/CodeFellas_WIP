using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prosjekt.Controllers;
using Xunit;

namespace Prosjekt.Xunit;

public class HomeControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        var controller = new HomeController(new ConfigurationBuilder().AddInMemoryCollection().Build());
        var result = controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void PrivacyPolicy_ReturnsViewResult()
    {
        var controller = new HomeController(new ConfigurationBuilder().AddInMemoryCollection().Build());
        var result = controller.PrivacyPolicy();
        Assert.IsType<ViewResult>(result);
    }
}
