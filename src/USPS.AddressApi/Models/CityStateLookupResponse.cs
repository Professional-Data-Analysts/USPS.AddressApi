using System.Xml.Linq;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the result of a CityStateLookup request from the USPS Address Api.
    /// </summary>
    public class CityStateLookupResponse : ResponseBase<CityStateLookupResult, ZipCode>
    {
        /// <summary>
        /// Creates a new instance of CityStateLookupResponse.
        /// </summary>
        public CityStateLookupResponse() : base() { }

        internal CityStateLookupResponse(XDocument responseDocument, params ZipCode[] originalZipCodes)
            : base(responseDocument, nameof(ZipCode), originalZipCodes)
        {

        }
    }
}