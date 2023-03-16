using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using USPS.AddressApi.Configuration;
using USPS.AddressApi.Models;

namespace USPS.AddressApi
{
    /// <summary>
    /// The abstraction that provides connectivity to the USPS Address RESTFul Api.
    /// </summary>
    public class AddressApiClient : IAddressApiClient
    {
        private readonly IAddressApiConnection _connection;
        private AddressApiClientOptions _options;
        private readonly ILogger<AddressApiClient> _logger;
        private readonly IOptionsMonitor<AddressApiClientOptions> _optionsMonitor;
        private readonly ILoggerFactory _internalLoggerFactory;

        /// <summary>
        /// Creates a new instance of AddressApiClient.
        /// </summary>
        /// <param name="options">The configuration options for the connection.</param>
        public AddressApiClient(AddressApiClientOptions options)
            : this(new AddressApiConnection(), options, null) { }


        internal AddressApiClient()
        {
            _internalLoggerFactory = LoggerFactory.Create(builder => { });

        }

        internal AddressApiClient(IAddressApiConnection connection, AddressApiClientOptions options, ILogger<AddressApiClient> logger)
            : this()
        {
            _connection = connection;
            _options = options;

            if(logger == null)
            {
                _logger = _internalLoggerFactory.CreateLogger<AddressApiClient>();
            }
            else
            {
                _logger = logger;
            }
        }

        internal AddressApiClient(IAddressApiConnection connection, IOptionsMonitor<AddressApiClientOptions> optionsMonitor, ILogger<AddressApiClient> logger)
            : this(connection, optionsMonitor?.CurrentValue, logger)
        {
            _optionsMonitor = optionsMonitor;
            _optionsMonitor.OnChange((opts) =>
            {
                _options = opts;
            });
        }

        /// <summary>
        /// Returns the ZIP Code and ZIP Code + 4 corresponding to the given address, city, and state (use USPS state abbreviations).
        /// </summary>
        /// <param name="addresses">The list of addresses to lookup. Minimum of 1, maximum of 5. Null values are not allowed.</param>
        public async Task<ZipCodeLookupResponse> LookupZipCodeAsync(params Address[] addresses)
        {
            if(addresses.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(addresses), "Address cannot be null.");
            }

            if(addresses.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(addresses), "You must supply at least one address to lookup.");
            }

            if(addresses.Length > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(addresses), "You can only lookup up to five addresses at a time.");
            }

            var element = new XElement(Constants.ZIPCODE_LOOKUP_REQUEST_ELEMENT_NAME);

            for (int i = 0; i < addresses.Length; i++)
            {
                element.Add(addresses[i].ToXElement(i));
                i++;
            }

            var requestDoc = new XDocument(element);
            requestDoc.Root.Add(new XAttribute(Constants.USERID_ATTRIBUTE_NAME, _options.UserId));

            var resultDoc = await ExecuteAsync(requestDoc, Constants.ZIPCODE_LOOKUP_OPERATION_NAME);
            var result = new ZipCodeLookupResponse(resultDoc, addresses);

            return result;
        }

        /// <summary>
        /// Returns the city and state corresponding to the given ZIP Code.
        /// </summary>
        /// <param name="zipCodes">The list of zip codes to lookup. Minimum of 1, maximum of 5. Null values are not allowed.</param>
        public async Task<CityStateLookupResponse> LookupCityStateAsync(params ZipCode[] zipCodes)
        {
            if(zipCodes.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(zipCodes), "Zip code cannot be null.");
            }
            if(zipCodes.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(zipCodes), "You must supply at least one zip code.");
            }

            if(zipCodes.Length > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(zipCodes), "You can only lookup five zip codes at a time.");
            }

            var element = new XElement(Constants.CITY_STATE_LOOKUP_REQUEST_ELEMENT_NAME);

            for (int i = 0; i < zipCodes.Length; i++)
            {
                element.Add(zipCodes[i].ToXElement(i));
                i++;
            }

            var requestDoc = new XDocument(element);
            requestDoc.Root.Add(new XAttribute(Constants.USERID_ATTRIBUTE_NAME, _options.UserId));

            var resultDoc = await ExecuteAsync(requestDoc, Constants.CITY_STATE_LOOKUP_OPERATION_NAME);
            var result = new CityStateLookupResponse(resultDoc, zipCodes);

            return result;
        }

        /// <summary>
        /// Corrects errors in street addresses, including abbreviations and missing information, and supplies ZIP Codes and ZIP Codes + 4.
        /// </summary>
        /// <param name="addresses">The list of addresses to verify. Minimum of 1, maximum of 5. Null values are not allowed.</param>
        public async Task<ValidateAddressResponse> ValidateAddressAsync(params Address[] addresses)
        {
            if(addresses.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(addresses), "Address cannot be null");
            }

            if(addresses.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(addresses), "You must supply at least one address to validate.");
            }

            if(addresses.Length > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(addresses), "You can only validate up to five addresses at a time.");
            }

            var element = new XElement(Constants.ADDRESS_VALIDATE_REQUEST_ELEMENT_NAME);
            element.Add(new XElement(Constants.REVISION_ELEMENT_NAME, 1));

            for (int i = 0; i < addresses.Length; i++)
            {
                element.Add(addresses[i].ToXElement(i));
                i++;
            }

            var requestDoc = new XDocument(element);
            requestDoc.Root.Add(new XAttribute(Constants.USERID_ATTRIBUTE_NAME, _options.UserId));

            var resultDoc = await ExecuteAsync(requestDoc, Constants.ADDRESS_VALIDATE_OPERATION_NAME);
            var result = new ValidateAddressResponse(resultDoc, addresses);

            return result;
        }

        private async Task<XDocument> ExecuteAsync(XDocument requestDoc, string action)
        {
            if(string.IsNullOrWhiteSpace(_options?.BaseApiUri))
            {
                throw new Exception("USPS base API URI was null or empty. Please ensure your configuration is correct");
            }

            if(!Uri.TryCreate(_options?.BaseApiUri, UriKind.Absolute, out var baseApiUri))
            {
                throw new Exception($"USPS base API URI is not a well-formatted URI. Please ensure your configuration is correct. The value of the supplied base API URI was: {_options?.BaseApiUri}");
            }

            var client = _connection.CreateHttpClient(baseApiUri);
            var uriBuilder = new UriBuilder(baseApiUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query[Constants.API_QUERY_PARAM_NAME] = action;
            query[Constants.XML_QUERY_PARAM_NAME] = requestDoc.ToString(SaveOptions.DisableFormatting);
            uriBuilder.Query = query.ToString();

            _logger.LogDebug("Preparing to execute the following request against the USPS Address API: {0}", uriBuilder.ToString());

            try
            {
                var response = await client.GetAsync(uriBuilder.Query);
                var stringResponse = await response.Content.ReadAsStringAsync();

                _logger.LogDebug("Response from the USPS Address API received. Status code: {0}, Response: {1}", response.StatusCode, stringResponse);

                return XDocument.Parse(stringResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to execute the following request against the USPS Address API: {0}", uriBuilder.ToString());
                throw;
            }
            
        }
    }
}