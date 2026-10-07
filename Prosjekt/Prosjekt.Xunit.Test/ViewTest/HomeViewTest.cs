//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;
//using Prosjekt.Controllers;
//using Microsoft.Extensions.Configuration;

//namespace Prosjekt.Xunit.Test.ViewTest
//{
//    public class HomeViewTest
//    {
//        private static IConfiguration CreateTestConfig()
//        {
//            return new ConfigurationBuilder()
//                .AddInMemoryCollection(new Dictionary<string, string?>
//                {
//                    ["ConnectionStrings:mysql"] = "server=127.0.0.1;port=1;Connect Timeout=1"
//                })
//                .Build();
//        }

//        [Fact]
//        public async Task Index_ReturnsViewResult()
//        {
//            // Arrange
//            var controller = new HomeController(CreateTestConfig());
//            // Act
//            var result = await controller.Index();
//            // Assert
//            Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result);
//        }
//        [Fact]
//        public async Task Index_ReturnsNotNull()
//        {
//            // Arrange
//            var controller = new HomeController(CreateTestConfig());
//            // Act
//            var result = await controller.Index();
//            // Assert
//            Assert.NotNull(result);
//        }
//    }
//}
