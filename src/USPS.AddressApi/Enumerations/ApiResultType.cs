using System.ComponentModel.DataAnnotations;

namespace USPS.AddressApi.Enumerations
{
    /// <summary>
    /// Represents the type of address-level result from a request to the USPS Address Api
    /// </summary>
    public enum ApiResultType
    {
        /// <summary>
        /// Indicates that a match was found for the address.
        /// </summary>
        [Display(Name = "Match")]
        Match,

        /// <summary>
        /// Indicates the address was found but more information is needed (such as an apartment, suite, or box number) to match to a specific address. 
        /// </summary>
        [Display(Name = "Default address: The address you entered was found but more information is needed (such as an apartment, suite, or box number) to match to a specific address.")]
        DefaultReturned,

        /// <summary>
        /// The address is invalid.
        /// </summary>
        [Display(Name = "Invalid Address.")]
        InvalidAddress,

        /// <summary>
        /// The zipcode value was invalid.
        /// </summary>
        [Display(Name = "Invalid Zip Code.")]
        InvalidZipCode,

        /// <summary>
        /// The zipcode length was invalid. Must be 5 digits.
        /// </summary>
        [Display(Name = "ZIPCode must be 5 characters")]
        InvalidZipCodeLength,

        /// <summary>
        /// The city value was invalid.
        /// </summary>
        [Display(Name = "Invalid City.")]
        InvalidCity,

        /// <summary>
        /// The two-digit state code was invalid.
        /// </summary>
        [Display(Name = "Invalid State Code.")]
        InvalidStateCode,

        /// <summary>
        /// The address was not found.
        /// </summary>
        [Display(Name = "Address Not Found.")]
        AddressNotFound,

        /// <summary>
        /// Multiple addresses were found for the address, and no default exists.
        /// </summary>
        [Display(Name = "Multiple addresses were found for the information you entered, and no default exists.")]
        MultipleFoundNoDefault,
        
        /// <summary>
        /// An unknown result was returned.
        /// </summary>
        UnknownResult
    }
}