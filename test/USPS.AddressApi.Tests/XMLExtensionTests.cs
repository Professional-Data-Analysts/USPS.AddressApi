using USPS.AddressApi.Extensions;

namespace USPS.AddressApi.Tests
{
    public class XMLExtensionTests
    {
        [Theory]
        [InlineData("test&", "test")]
        [InlineData("&", "")]
        [InlineData("*,.()\":;'-@&<>", "")]
        [InlineData("&&test,.tes*t>", "testtest")]
        public void Should_Remove_Invalid_Characters(string testString, string expectedResult)
        {
            // Arrange

            // Act
            var result = testString.Clean();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}