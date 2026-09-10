using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit.Test.ModelTest
{
    public class RessursModelTest
    {
        [Fact]
        public void RessursViewModel_DefaultValues_AreSetCorrectly()
        {
            // Act
            var model = new RessursViewModel();

            // Assert
            Assert.Equal(string.Empty, model.Navn);
            Assert.Equal(string.Empty, model.Beskrivelse);
            Assert.Equal(0, model.Antall);
        }

        [Fact]
        public void RessursViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var model = new RessursViewModel
            {
                Navn = "Test Ressurs",
                Beskrivelse = "Dette er en testressurs.",
                Antall = 5
            };

            // Assert
            Assert.Equal("Test Ressurs", model.Navn);
            Assert.Equal("Dette er en testressurs.", model.Beskrivelse);
            Assert.Equal(5, model.Antall);
        }
    }
}