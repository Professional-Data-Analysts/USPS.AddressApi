using System;
using System.Xml.Linq;
using USPS.AddressApi.Extensions;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents a Zip Code.
    /// </summary>
    public class ZipCode
    {
        /// <summary>
        /// The 5-digit US ZIP Code. If international, value must be 00000. Example: 55111
        /// </summary>
        public string Zip5 { get; set; }

        /// <summary>
        /// Creates a new instance of ZipCode.
        /// </summary>
        public ZipCode() { }

        internal ZipCode(XElement element)
        {
            if(element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            Zip5 = element.Element(nameof(Zip5))?.Value;
        }

        internal virtual XElement ToXElement(int trackingId)
        {
            return new XElement(nameof(ZipCode),
                new XAttribute(Constants.ID_ATTRIBUTE_NAME, trackingId.ToString()),
                new XElement(nameof(Zip5), Zip5?.Clean()));
        }
    }
}