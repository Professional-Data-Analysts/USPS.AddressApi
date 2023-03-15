using System.Xml.Linq;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the item-level result of a CityStateLookup request from the USPS Address Api.
    /// </summary>
    public class CityStateLookupResult : ResultBase<ZipCode>
    {
        /// <summary>
        /// City returned for the given zip code.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State returned for the given zip code. A two letter enumeration will return for the given state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The 5-digit US ZIP Code. If international, value must be 00000. Example: 55111
        /// </summary>
        public string Zip5 { get; set; }

        /// <summary>
        /// Creates a new instance of CityStateLookupResult.
        /// </summary>
        public CityStateLookupResult() : base() { }

        internal CityStateLookupResult(XElement element, ZipCode original)
            : base(element, original)
        {
            Zip5 = element.Element(nameof(Zip5))?.Value;
            City = element.Element(nameof(City))?.Value;
            State = element.Element(nameof(State))?.Value;
        }
    }
}