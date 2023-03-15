using System.ComponentModel.DataAnnotations;

namespace USPS.AddressApi.Enumerations
{
    /// <summary>
    /// DPV® Standardized Footnotes - EZ24x7Plus and Mail*STAR are required to express DPV results using USPS standard two character footnotes
    /// </summary>
    public enum DPVFootnoteCodeType
    {
        /// <summary>
        /// Input address matched to the ZIP+4 file. Reports CASS™ ZIP+4™ Certification.
        /// </summary>
        [Display(Name = "AA")]
        InputMatchedToZip4File,

        /// <summary>
        /// Input address not matched to the ZIP+4 file. Reports CASS™ ZIP+4™ Certification.
        /// </summary>
        [Display(Name = "A1")]
        InputNotMatchedToZip4File,

        /// <summary>
        /// Matched to DPV (all components). Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "BB")]
        MatchedAllDVPComponents,

        /// <summary>
        /// Secondary number not matched (present but invalid). Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "CC")]
        SecondaryNumberNotMatched,

        /// <summary>
        /// High-rise address missing secondary number. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "N1")]
        HighRiseAddressMissingSecondaryNumber,

        /// <summary>
        /// Primary number missing. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "M1")]
        PrimaryNumberMissing,

        /// <summary>
        /// Primary number invalid. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "M3")]
        PrimaryNumberInvalid,

        /// <summary>
        /// Input Address RR or HC Box number Missing. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "P1")]
        InputAddress_RR_HC_Missing,

        /// <summary>
        /// Input Address PO, RR, or HC Box number invalid. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "P3")]
        InputAddress_PO_RR_HC_NumberInvalid,

        /// <summary>
        /// Input Address Matched to a military address. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "F1")]
        InputAddressMatchedMilitaryAddress,

        /// <summary>
        /// Input Address Matched to a General Delivery Address. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "G1")]
        InputAddressMatchedGeneralDeliveryAddress,
        
        /// <summary>
        /// Input Address Matched to a Unique ZIP Code™. Reports DPV Validation Observations.
        /// </summary>
        [Display(Name = "U1")]
        InputAddressMatchedUniqueZIPCode
    }
}