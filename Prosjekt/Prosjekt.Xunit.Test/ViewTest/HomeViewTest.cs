using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Prosjekt.Controllers;

namespace Prosjekt.Xunit.Test.ViewTest
{
    public class HomeViewTest
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController();
            // Act
            var result = controller.Index();
            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result);
        }
        [Fact]
        public void Index_ReturnsNotNull()
        {
            // Arrange
            var controller = new HomeController();
            // Act
            var result = controller.Index();
            // Assert
            Assert.NotNull(result);
        }
    }
}
