using System.Xml.Linq;
using USPS.AddressApi.Enumerations;
using USPS.AddressApi.Extensions;

namespace USPS.AddressApi.Tests
{
    public class EnumExtensionTests
    {
        [Theory]
        [InlineData("test", false)]
        [InlineData("InvalidAddress", true)]
        [InlineData("UnknownResult", true)]
        public void Should_Parse_Enum_From_Name(string name, bool shouldMatch)
        {
            // Arrange

            // Act
            var success = name.TryGetEnumFromName<ApiResultType>(out var result);
            
            // Assert
            Assert.Equal(shouldMatch, success);
        }

        [Theory]
        [InlineData("test", false)]
        [InlineData("Invalid Address.", true)]
        public void Should_Parse_Enum_From_Display_Name(string name, bool shouldMatch)
        {
            // Arrange

            // Act
            var success = name.TryGetEnumFromName<ApiResultType>(out var result);
            
            // Assert
            Assert.Equal(shouldMatch, success);
        }

        [Theory]
        [InlineData("test", false)]
        [InlineData("Invalid Address.", true)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void Should_Parse_Enum_From_XElement_By_Display_Name(string name, bool shouldMatch)
        {
            // Arrange
            var element = new XElement("Parent", new XElement("Child", name));

            // Act
            var success = element.TryGetEnumFromChildElement<ApiResultType>("Child", out _);

            // Assert
            Assert.Equal(shouldMatch, success);
        }
    }
}