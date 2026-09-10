using System;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Controllers;
using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit.Test.ControllerTest
{
    public class RessursControllerTest
    {
        private readonly RessursController _controller;

        public RessursControllerTest()
        {
            _controller = new RessursController();
        }

        [Fact]
        public void Index_ReturnsViewResultWithModel()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RessursViewModel>(viewResult.Model);
        }

        [Fact]
        public void Create_ValidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new RessursViewModel { Navn = "Test", Beskrivelse = "Test", Antall = 1 };

            // Act
            var result = _controller.Create(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }
    }
}