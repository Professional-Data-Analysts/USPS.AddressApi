using System.Xml.Linq;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the item-level result of a ZipCodeLookup request from the USPS Address Api.
    /// </summary>
    public class ZipCodeLookupResult : ResultBase<Address>
    {
        /// <summary>
        /// The address prefix. Example: SUITE K
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// The address. Example: 1234 Somplace St E
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// The name of the city. Example: Minneapolis
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The name of the business or firm. Example: XYZ Corp.
        /// </summary>
        public string FirmName { get; set; }

        /// <summary>
        /// The two character state code of the address. Example: MN
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// For Puerto Rico addresses only.
        /// </summary>
        public string Urbanization { get; set; }

        /// <summary>
        /// The 5-digit US ZIP Code. If international, value must be 00000. Example: 55111
        /// </summary>
        public string Zip5 { get; set; }

        /// <summary>
        /// The ZIP+4 ZIP Code. If international, value must be 0000. Example: 8765
        /// </summary>
        public string Zip4 { get; set; }

        /// <summary>
        /// Creates a new instance of ZipCodeLookupResult.
        /// </summary>
        public ZipCodeLookupResult() : base() { }

        internal ZipCodeLookupResult(XElement element, Address original)
            : base(element, original)
        {
            Address1 = element.Element(nameof(Address1))?.Value;
            Address2 = element.Element(nameof(Address2))?.Value;
            City = element.Element(nameof(City))?.Value;
            FirmName = element.Element(nameof(FirmName))?.Value;
            State = element.Element(nameof(State))?.Value;
            Urbanization = element.Element(nameof(Urbanization))?.Value;
            Zip5 = element.Element(nameof(Zip5))?.Value;
            Zip4 = element.Element(nameof(Zip4))?.Value;
        }
    }
}