using xe_ideas.Models;
using xe_ideas.Services.Interfaces;
using xe_ideas.Services.LookUp;
using Xunit;

namespace xe_ideas.Tests.Services.LookUp
{
    public class LookUpServiceTests
    {
        private readonly ILookUpService _lookUpService;

        public LookUpServiceTests()
        {
            this._lookUpService = new LookUpService();
        }

        [Fact]
        public void GetAll_ShouldReturnAllLookUpValues()
        {
            // Arrange
            ApplicationContext fakeContext = null;
            string IDEA_PRIVACIES = "IdeaPrivacies";

            // Act
            var result = this._lookUpService.GetAll(fakeContext);

            // Assert
            Assert.NotNull(result[IDEA_PRIVACIES]);
        }
    }
}