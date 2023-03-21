namespace USPS.AddressApi.Configuration
{
    /// <summary>
    /// Represents the configuration options required for interacting with the USPS Address Api.
    /// </summary>
    public class AddressApiClientOptions
    {
        /// <summary>
        /// The name of the configuration section.
        /// </summary>
        public const string CONFIGURATION_SECTION_NAME = "USPS.AddressApi";

        /// <summary>
        /// The default base uri for the USPS Address Information Api service.
        /// </summary>
        public const string DEFAULT_API_BASE_URI = "https://secure.shippingapis.com/ShippingAPI.dll";

        /// <summary>
        /// Your unique account key used to identify you to the USPS Address Api.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The base uri of the USPS Address Api service. Defaults to: https://secure.shippingapis.com/ShippingAPI.dll
        /// </summary>
        public string BaseApiUri { get; set; } = DEFAULT_API_BASE_URI;
    }
}