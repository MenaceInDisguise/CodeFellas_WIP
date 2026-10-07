using Prosjekt.Models.ModelView;
using Xunit;

namespace Prosjekt.Xunit.Test.ModelTest
{
    public class ResourceModelTest
    {
        [Fact]
        public void ResourceViewModel_DefaultValues_AreSetCorrectly()
        {
            // Act
            var model = new ResourceViewModel();

            // Assert
            Assert.Equal(string.Empty, model.Name);
            Assert.Equal(string.Empty, model.Description);
            Assert.Equal(0, model.Quantity);
        }

        [Fact]
        public void ResourceViewModel_SetProperties_ReturnsCorrectValues()
        {
            // Arrange & Act
            var model = new ResourceViewModel
            {
                Name = "Test Resource",
                Description = "This is a test resource.",
                Quantity = 5
            };

            // Assert
            Assert.Equal("Test Resource", model.Name);
            Assert.Equal("This is a test resource.", model.Description);
            Assert.Equal(5, model.Quantity);
        }
    }
}