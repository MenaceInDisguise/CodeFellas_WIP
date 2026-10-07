//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Prosjekt.Controllers;
//using Prosjekt.Models.ModelView;
//using Xunit;
//using Microsoft.Extensions.Configuration;

//namespace Prosjekt.Xunit
//{
//    public class HomeControllerTest
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
//            var controller = new HomeController(CreateTestConfig());

//            var result = await controller.Index();

//            Assert.IsType<ViewResult>(result);
//        }

//        [Fact]
//        public void PrivacyPolicy_ReturnsViewResult()
//        {
//            var controller = new HomeController(CreateTestConfig());

//            var result = controller.PrivacyPolicy();

//            Assert.IsType<ViewResult>(result);
//        }

//        [Fact]
//        public void Error_ReturnsViewResultWithModel()
//        {
//            var controller = new HomeController(CreateTestConfig());
//            controller.ControllerContext = new ControllerContext
//            {
//                HttpContext = new DefaultHttpContext()
//            };

//            var result = controller.Error();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
//            Assert.NotNull(model.RequestId);
//        }
//    }
//}