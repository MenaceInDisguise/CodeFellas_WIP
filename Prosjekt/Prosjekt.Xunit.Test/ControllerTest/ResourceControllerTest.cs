//using System;
//using Microsoft.AspNetCore.Mvc;
//using Prosjekt.Controllers;
//using Prosjekt.Models.ModelView;
//using Xunit;

//namespace Prosjekt.Xunit.Test.ControllerTest
//{
//    public class ResourceControllerTest
//    {
//        private readonly ResourceController _controller;

//        public ResourceControllerTest()
//        {
//            _controller = new ResourceController();
//        }

//        [Fact]
//        public void Index_ReturnsViewResultWithModel()
//        {
//            // Act
//            var result = _controller.Index();

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.IsType<ResourceViewModel>(viewResult.Model);
//        }

//        [Fact]
//        public void Create_ValidModel_ReturnsViewResult()
//        {
//            // Arrange
//            var model = new ResourceViewModel
//            {
//                Name = "Test",
//                Description = "Test",
//                Quantity = 1,
//                Latitude = "58.146700",
//                Longitude = "7.995600"
//            };

//            //// Act
//            //var result = _controller.Create(model);

//            // Assert
//        //    var viewResult = Assert.IsType<ViewResult>(result);
//        //    Assert.Equal(model, viewResult.Model);
//        //}
//    }
//}