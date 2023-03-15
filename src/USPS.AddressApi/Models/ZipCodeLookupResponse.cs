using System.Xml.Linq;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the result of a ZipCodeLookup request from the USPS Address Api.
    /// </summary>
    public class ZipCodeLookupResponse : ResponseBase<ZipCodeLookupResult, Address>
    {
        /// <summary>
        /// Creates a new instance of ZipCodeLookupResponse.
        /// </summary>
        public ZipCodeLookupResponse() : base() { }

        internal ZipCodeLookupResponse(XDocument responseDocument, params Address[] originalAddresses)
            : base(responseDocument, nameof(Address), originalAddresses)
        {
           
        }
    }
}