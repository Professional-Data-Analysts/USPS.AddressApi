using System.Xml.Linq;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the result of a ValidateAddress request from the USPS Address Api.
    /// </summary>
    public class ValidateAddressResponse : ResponseBase<ValidateAddressResult, Address>
    {
        /// <summary>
        /// Creates a new instance of ValidateAddressResponse.
        /// </summary>
        public ValidateAddressResponse() : base() { }

        internal ValidateAddressResponse(XDocument responseDocument, params Address[] originalAddresses)
            : base(responseDocument, nameof(Address), originalAddresses)
        {
             
        }
    }
}