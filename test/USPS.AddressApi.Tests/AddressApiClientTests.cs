using USPS.AddressApi.Models;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using USPS.AddressApi.Configuration;

namespace USPS.AddressApi.Tests
{
    public class AddressApiClientTests
    {
        private const string API_URI = "https://localhost/TestAPI.dll";
        private const string XML_CONTENT_TYPE = "text/xml";

#region Validate Address

    [Fact]
    public async Task Validate_Address_Should_Not_Accept_Null_Parameter()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();

        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => { return api.ValidateAddressAsync(null); });
    }

    [Fact]
    public async Task Validate_Address_Should_Not_Accept_Empty_Parameter_List()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();
        
        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => { return api.ValidateAddressAsync(); });
    }

    [Fact]
    public async Task Validate_Address_Should_Not_Accept_Too_Many_Parameter_List()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();
        
        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => { return api.ValidateAddressAsync(
            new(), new(), new(), new(), new(), new()
        ); });
    }

    [Fact]
    public async Task Validate_Address_Should_Execute_Correct_Request()
    {
        // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var expectedRequestPayload = await File.ReadAllTextAsync(Path.Combine("Data", "ValidateAddress", "ValidateAddressRequest_Payload.xml"));
        var response = await File.ReadAllTextAsync(Path.Combine("Data", "ValidateAddress", "ValidateAddressResponse_Success.xml"));
        var request = new Address[]
        {
            new Address() {
                Address1 = "TEST_ADDRESS1",
                Address2 = "TEST_ADDRESS2",
                City = "TEST_CITY",
                State = "MN",
                Urbanization = "TEST_URBANIZATION",
                Zip4 = "TEST_ZIP4",
                Zip5 = "TEST_ZIP5"
            }
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.XML_QUERY_PARAM_NAME, expectedRequestPayload)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ADDRESS_VALIDATE_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, response);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        await api.ValidateAddressAsync(request);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task Validate_Address_Should_Match_Batch_Order_Correctly()
    {
        // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "ValidateAddress", "ValidateAddressBatchResponse_Out_Of_Order_Items.xml"));
        var request = new Address[]
        {
            new() { Address1 = "3"},
            new() { Address1 = "4"},
            new() { Address1 = "1"},
            new() { Address1 = "2"}
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ADDRESS_VALIDATE_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.ValidateAddressAsync(request);

        // Assert
        Assert.True(request.Select(x=>x.Address1).ToArray().SequenceEqual(response.Results.Select(y=>y.Address1).ToArray()));
    }

    [Fact]
    public async Task Validate_Address_Lookup_Should_Catch_Top_Level_Api_Errors()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "ValidateAddress", "ValidateAddressResponse_TopLevelError.xml"));
        var request = new Address[]
        {
            new()
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ADDRESS_VALIDATE_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.ValidateAddressAsync(request);

        // Assert
        Assert.True(response.Error != null);
    }

    [Fact]
    public async Task Validate_Address_Lookup_Should_Catch_Item_Level_Errors()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "ValidateAddress", "ValidateAddressResponse_ItemLevelError.xml"));
        var request = new Address[]
        {
            new(),
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ADDRESS_VALIDATE_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.ValidateAddressAsync(request);

        // Assert
        Assert.True(response.Results[0].ResultType == Enumerations.ApiResultType.InvalidCity);
    }

#endregion

#region ZipCode Lookup

    [Fact]
    public async Task ZipCode_Lookup_Should_Not_Accept_Null_Parameter()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();

        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => { return api.LookupZipCodeAsync(null); });
    }

    [Fact]
    public async Task ZipCode_Lookup_Should_Not_Accept_Empty_Parameter_List()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();
        
        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => { return api.LookupZipCodeAsync(); });
    }

    [Fact]
    public async Task ZipCode_Lookup_Should_Not_Accept_Too_Many_Parameter_List()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();
        
        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => { return api.LookupZipCodeAsync(
            new(), new(), new(), new(), new(), new()
        ); });
    }

    [Fact]
    public async Task ZipCode_Lookup_Should_Execute_Correct_Request()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var expectedRequestPayload = await File.ReadAllTextAsync(Path.Combine("Data", "ZipCodeLookup", "ZipCodeLookupRequest_Payload.xml"));
        var response = await File.ReadAllTextAsync(Path.Combine("Data", "ZipCodeLookup", "ZipCodeLookupResponse_Success.xml"));
        var request = new Address()
        {
            Address1 = "TEST_ADDRESS1",
            Address2 = "TEST_ADDRESS2",
            City = "TEST_CITY",
            State = "MN",
            Zip4 = "TEST_ZIP4",
            Zip5 = "TEST_ZIP5"
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString("XML", expectedRequestPayload)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ZIPCODE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, response);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        await api.LookupZipCodeAsync(request);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task ZipCode_Lookup_Should_Match_Batch_Order_Correctly()
    {
        // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "ZipCodeLookup", "ZipCodeLookupBatchResponse_Out_Of_Order_Items.xml"));
        var request = new Address[]
        {
            new() { Address1 = "3"},
            new() { Address1 = "4"},
            new() { Address1 = "1"},
            new() { Address1 = "2"}
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ZIPCODE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.LookupZipCodeAsync(request);

        // Assert
        Assert.True(request.Select(x=>x.Address1).ToArray().SequenceEqual(response.Results.Select(y=>y.Address1).ToArray()));
    }

    [Fact]
    public async Task Zip_Code_Lookup_Should_Catch_Top_Level_Api_Errors()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "ZipCodeLookup", "ZipCodeLookupResponse_TopLevelError.xml"));
        var request = new ZipCode[]
        {
            new() { Zip5 = "3"},
            new() { Zip5 = "4"},
            new() { Zip5 = "1"},
            new() { Zip5 = "2"}
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.CITY_STATE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.LookupCityStateAsync(request);

        // Assert
        Assert.True(response.Error != null);
    }

    [Fact]
    public async Task Zip_Code_Lookup_Should_Catch_Item_Level_Errors()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "ZipCodeLookup", "ZipCodeLookupResponse_ItemLevelError.xml"));
        var request = new Address[]
        {
            new()
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.ZIPCODE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.LookupZipCodeAsync(request);

        // Assert
        Assert.True(response.Results[0].ResultType == Enumerations.ApiResultType.InvalidCity);
    }

#endregion

#region City State Lookup

    [Fact]
    public async Task City_State_Lookup_Should_Not_Accept_Null_Parameter()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();

        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => { return api.LookupCityStateAsync(null); });
    }

    [Fact]
    public async Task City_State_Should_Not_Accept_Empty_Parameter_List()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();
        
        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => { return api.LookupCityStateAsync(); });
    }

    [Fact]
    public async Task City_State_Should_Not_Accept_Too_Many_Parameter_List()
    {
        // Arrange
        var api = TestHelpers.CreateDefaultApiInstance();
        
        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => { return api.LookupCityStateAsync(
            new(), new(), new(), new(), new(), new()
        ); });
    }

    [Fact]
    public async Task City_State_Lookup_Should_Execute_Correct_Request()
    {
        // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var expectedRequestPayload = await File.ReadAllTextAsync(Path.Combine("Data", "CityStateLookup", "CityStateLookupRequest_Payload.xml"));
        var response = await File.ReadAllTextAsync(Path.Combine("Data", "CityStateLookup", "CityStateLookupResponse_Success.xml"));
        var request = new ZipCode
        {
            Zip5 = "TEST_ZIP5"
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.XML_QUERY_PARAM_NAME, expectedRequestPayload)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.CITY_STATE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, response);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        await api.LookupCityStateAsync(request);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task City_State_Lookup_Should_Match_Batch_Order_Correctly()
    {
        // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "CityStateLookup", "CityStateLookupBatchResponse_Out_Of_Order_Items.xml"));
        var request = new ZipCode[]
        {
            new() { Zip5 = "3"},
            new() { Zip5 = "4"},
            new() { Zip5 = "1"},
            new() { Zip5 = "2"}
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.CITY_STATE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.LookupCityStateAsync(request);

        // Assert
        Assert.True(request.Select(x=>x.Zip5).ToArray().SequenceEqual(response.Results.Select(y=>y.Zip5).ToArray()));
    }

    [Fact]
    public async Task City_State_Lookup_Should_Catch_Top_Level_Api_Errors()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "CityStateLookup", "CityStateLookupResponse_TopLevelError.xml"));
        var request = new ZipCode[]
        {
            new() { Zip5 = "3"},
            new() { Zip5 = "4"},
            new() { Zip5 = "1"},
            new() { Zip5 = "2"}
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.CITY_STATE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.LookupCityStateAsync(request);

        // Assert
        Assert.True(response.Error != null);
    }

    [Fact]
    public async Task City_State_Lookup_Should_Catch_Item_Level_Errors()
    {
            // Arrange
        var connection = new Mock<IAddressApiConnection>();
        var options = new AddressApiClientOptions() { UserId = "TESTUSER123" };
        var mockHttp = new MockHttpMessageHandler();
        var mockLogger = new Mock<ILogger<AddressApiClient>>();
        var responseData = await File.ReadAllTextAsync(Path.Combine("Data", "CityStateLookup", "CityStateLookupResponse_ItemLevelError.xml"));
        var request = new ZipCode[]
        {
            new() { Zip5 = "1"},
        };

        mockHttp.Expect(HttpMethod.Get, API_URI)
        .WithQueryString(Constants.API_QUERY_PARAM_NAME, Constants.CITY_STATE_LOOKUP_OPERATION_NAME)
        .Respond(XML_CONTENT_TYPE, responseData);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(API_URI);

        connection.Setup(x => x.CreateHttpClient(It.IsAny<Uri>())).Returns(client);

        var api = new AddressApiClient(connection.Object, options, mockLogger.Object);

        // Act
        var response = await api.LookupCityStateAsync(request);

        // Assert
        Assert.True(response.Results[0].ResultType == Enumerations.ApiResultType.InvalidCity);
    }

#endregion
 
    }
}