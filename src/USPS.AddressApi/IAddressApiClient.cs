using System.Threading.Tasks;
using USPS.AddressApi.Models;

namespace USPS.AddressApi
{
    /// <summary>
    /// The wrapper which enables managed interaction with the USPS Address Api
    /// </summary>
    public interface IAddressApiClient
    {
        /// <summary>
        /// Validates up to five <see cref="Address"/> via the USPS Address Api
        /// </summary>
        /// <param name="addresses">The addresses to validate (up to 5)</param>
        Task<ValidateAddressResponse> ValidateAddressAsync(params Address[] addresses);

        /// <summary>
        /// Returns the zipcode and zip + 4 code for a given address.
        /// </summary>
        /// <param name="request">The address to lookup (up to 5)</param>
        Task<ZipCodeLookupResponse> LookupZipCodeAsync(params Address[] addresses);

        /// <summary>
        /// Returns the city and state for a given zipcode.
        /// </summary>
        /// <param name="request">The city and state to lookup (up to 5)</param>
        Task<CityStateLookupResponse> LookupCityStateAsync(params ZipCode[] zipCodes);
    }
}