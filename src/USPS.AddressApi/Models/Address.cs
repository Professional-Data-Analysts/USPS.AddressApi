using System.Xml.Linq;
using USPS.AddressApi.Extensions;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents details of a given mailing or physical address.
    /// </summary>
    public class Address
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
        /// Creates a new instance of an Address.
        /// </summary>
        public Address() { }

        internal virtual XElement ToXElement(int trackingId)
        {
            return new XElement(nameof(Address),
                new XAttribute(Constants.ID_ATTRIBUTE_NAME, trackingId.ToString()),
                new XElement(nameof(FirmName), FirmName?.Clean()),
                new XElement(nameof(Address1), Address1?.Clean()),
                new XElement(nameof(Address2), Address2?.Clean()),
                new XElement(nameof(City), City?.Clean()),
                new XElement(nameof(State), State?.Clean()),
                new XElement(nameof(Urbanization), Urbanization?.Clean()),
                new XElement(nameof(Zip5), Zip5?.Clean()),
                new XElement(nameof(Zip4), Zip4?.Clean()));
        }
    }
}