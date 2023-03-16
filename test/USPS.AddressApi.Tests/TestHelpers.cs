using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using USPS.AddressApi.Configuration;

namespace USPS.AddressApi.Tests
{
    public class TestHelpers
    {

        public static AddressApiClient CreateDefaultApiInstance()
        {
            var connection = new Mock<IAddressApiConnection>();
            var options = new AddressApiClientOptions() { UserId = "TEST123" };
            var mockHttp = new MockHttpMessageHandler();
            var mockLogger = new Mock<ILogger<AddressApiClient>>();

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://localhost/ShippingAPI.dll");

            mockHttp.When("https://localhost/ShippingAPI.dll*").Respond("text/xml", string.Empty);
            connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

            return new AddressApiClient(connection.Object, options, mockLogger.Object);
        }

        public static AddressApiClient CreateApiInstanceWithResponseFromFile(string returnPayloadFileName, string userId)
        {
            var xml = File.ReadAllText(Path.Combine("Data", returnPayloadFileName));
            var connection = new Mock<IAddressApiConnection>();
            var options = new AddressApiClientOptions() { UserId = userId };
            var mockHttp = new MockHttpMessageHandler();
            var mockLogger = new Mock<ILogger<AddressApiClient>>();

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://localhost/ShippingAPI.dll");

            mockHttp.When("https://localhost/ShippingAPI.dll*").Respond("text/xml", xml);
            connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

            return new AddressApiClient(connection.Object, options, mockLogger.Object);
        }


        public static AddressApiClient CreateApiInstanceWithEmptyResponse(string userId)
        {
            var connection = new Mock<IAddressApiConnection>();
            var options = new AddressApiClientOptions() { UserId = userId };
            var mockHttp = new MockHttpMessageHandler();
            var mockLogger = new Mock<ILogger<AddressApiClient>>();

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://localhost/ShippingAPI.dll");

            mockHttp.When("https://localhost/ShippingAPI.dll*").Respond("text/xml", string.Empty);
            connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

            return new AddressApiClient(connection.Object, options, mockLogger.Object);
        }

        public static Tuple<MockHttpMessageHandler, IAddressApiClient> CreateMockHandler(string userId, string response)
        {
            var connection = new Mock<IAddressApiConnection>();
            var options = new AddressApiClientOptions() { UserId = userId };
            var mockHttp = new MockHttpMessageHandler();
            var mockLogger = new Mock<ILogger<AddressApiClient>>();

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://secure.shippingapis.com/ShippingAPI.dll");

            mockHttp.When("*").Respond("text/xml", response);
            connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

            var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

            return new (mockHttp, api);
        }
    }
}