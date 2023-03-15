using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using USPS.AddressApi.Enumerations;
using USPS.AddressApi.Extensions;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the item-level result of a ValidateAddress request from the USPS Address Api.
    /// </summary>
    public class ValidateAddressResult : ResultBase<Address>
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
        /// The abbreviation for Address2.
        /// </summary>
        public string Address2Abbreviation { get; set; }

        /// <summary>
        /// The abbreviated city name.
        /// </summary>
        public string CityAbbreviation { get; set; }

        /// <summary>
        /// The delivery point information.
        /// </summary>
        public string DeliveryPoint { get; set; }

        /// <summary>
        /// The carrier route number.
        /// </summary>
        public string CarrierRoute { get; set; }

        /// <summary>
        /// Flags that indicate the nature of the match and any notes pertaining to the address as it was entered.
        /// </summary>
        public FootnoteCodeType[] FootNotes { get; set; }

        /// <summary>
        /// The DPV Confirmation Indicator is the primary method used by the USPS to determine whether an address was considered deliverable or undeliverable. 
        /// </summary>
        public DPVConfirmationCodeType? DPVConfirmation { get; set; }

        /// <summary>
        /// CMRA Indicates a private business that acts as a mail-receiving agent for specific clients.
        /// A value of true indicates the address was found in the CMRA table.
        /// A value of false indicates the address was not found in the CMRA table.
        /// A null value indicates the address was not presented in the table.
        /// </summary>
        public bool? IsDPVCMRA { get; set; }

        /// <summary>
        /// DPV® Standardized Footnotes - EZ24x7Plus and Mail*STAR are required to express DPV results using USPS standard two character footnotes.
        /// </summary>
        public DPVFootnoteCodeType[] DPVFootnotes { get; set; }

        /// <summary>
        /// Central Delivery is for all business office buildings, office complexes, and/or industrial/professional parks. This may include call windows, horizontal locked mail receptacles, cluster box units.
        /// </summary>
        public bool? IsCentralDeliveryPoint { get; set; }

        /// <summary>
        /// Indicates whether address is a business or not
        /// </summary>
        public bool? IsBusiness { get; set; }

        /// <summary>
        /// Indicates if the location is occupied or not.
        /// </summary>
        public bool? IsVacant { get; set; }

        /// <summary>
        /// Creates a new instance of ValidateAddressResult.
        /// </summary>
        public ValidateAddressResult() : base() { }

        internal ValidateAddressResult(XElement element, Address original)
            :base(element, original)
        {
            Address1 = element.Element(nameof(Address1))?.Value;
            Address2 = element.Element(nameof(Address2))?.Value;
            City = element.Element(nameof(City))?.Value;
            FirmName = element.Element(nameof(FirmName))?.Value;
            State = element.Element(nameof(State))?.Value;
            Urbanization = element.Element(nameof(Urbanization))?.Value;
            Zip5 = element.Element(nameof(Zip5))?.Value;
            Zip4 = element.Element(nameof(Zip4))?.Value;
            Address2Abbreviation = element.Element(nameof(Address2Abbreviation))?.Value;
            CityAbbreviation = element.Element(nameof(CityAbbreviation))?.Value;
            CarrierRoute = element.Element(nameof(CarrierRoute))?.Value;
            DeliveryPoint = element.Element(nameof(DeliveryPoint))?.Value;

            if(element.TryGetChildString(Constants.RETURN_TEXT_ELEMENT_NAME, out var returnText))
            {
                ResultType = ApiResultType.DefaultReturned;
            }

            if(element.TryGetChildString(Constants.DPVCMRA_ELEMENT_NAME, out var dpvcrma))
            {
                IsDPVCMRA = dpvcrma == Constants.YES_VALUE ? true : false;
            }

            if(element.TryGetChildString(Constants.BUSINESS_ELEMENT_NAME, out var business))
            {
                IsBusiness = business == Constants.YES_VALUE ? true : false;
            }

            if(element.TryGetChildString(Constants.CENTRAL_DELIVERY_POINT_ELEMENT_NAME, out var deliveryPoint))
            {
                IsCentralDeliveryPoint = deliveryPoint == Constants.YES_VALUE ? true : false;
            }

            if(element.TryGetChildString(Constants.VACANT_ELEMENT_NAME, out var vacant))
            {
                IsVacant = vacant == Constants.YES_VALUE ? true : false;
            }

            if(element.TryGetChildString(Constants.FOOTNOTES_ELEMENT_NAME, out var footnotes))
            {
                var footnotesList = new List<FootnoteCodeType>();
                foreach(var fn in footnotes.ToArray())
                {
                    if(fn.ToString().TryGetEnumFromName<FootnoteCodeType>(out var footnote))
                    {
                        footnotesList.Add(footnote);
                    }
                }

                FootNotes = footnotesList.ToArray();
            }

            if(element.TryGetEnumFromChildElement<DPVConfirmationCodeType>(Constants.DPV_CONFIRMATION_NAME, out var code))
            {
                DPVConfirmation = code;
            }

            if(element.TryGetChildString(nameof(DPVFootnotes), out var dpvFootnotes))
            {
                var dpvFootnotesList = Enumerable.Range(0, dpvFootnotes.Length / 2)
                        .Select(i => dpvFootnotes.Substring(i * 2, 2));
                var dpvFootnotesCodeList = new List<DPVFootnoteCodeType>();

                foreach(var item in dpvFootnotesList)
                {
                    if(item.TryGetEnumFromName<DPVFootnoteCodeType>(out var footnoteCode))
                    {
                        dpvFootnotesCodeList.Add(footnoteCode);
                    }
                }

                DPVFootnotes = dpvFootnotesCodeList.ToArray();
            }
        }
    }
}