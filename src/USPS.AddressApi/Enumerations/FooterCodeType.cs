using System.ComponentModel.DataAnnotations;

namespace USPS.AddressApi.Enumerations
{
    /// <summary>
    /// Indicates aspects or corrections of the address-level result returned from the USPS Address Api.
    /// </summary>
    public enum FootnoteCodeType
    {
        /// <summary>
        /// The address was found to have a different 5-digit Zip Code than given in the submitted list. The correct Zip Code is shown in the output address.
        /// </summary>
        [Display(Name = "A")]
        ZipCodeCorrected,

        /// <summary>
        /// The spelling of the city name and/or state abbreviation in the submitted address was found to be different than the standard spelling. The standard spelling of the city name and state abbreviation are shown in the output address.
        /// </summary>
        [Display(Name = "B")]
        CityStateSpellingCorrected,

        /// <summary>
        /// The Zip Code in the submitted address could not be found because neither a valid city, state, nor valid 5-digit Zip Code was present. It is also recommended that the requestor check the submitted address for accuracy. 
        /// </summary>
        [Display(Name = "C")]
        InvalidCityStateZip,

        /// <summary>
        /// This is a record listed by the United State Postal Service on the national Zip+4 file as a non-deliverable location. It is recommended that the requestor verify the accuracy of the submitted address.
        /// </summary>
        [Display(Name = "D")]
        NoZip4Assigned,

        /// <summary>
        /// Multiple records were returned, but each shares the same 5-digit Zip Code.
        /// </summary>
        [Display(Name = "E")]
        ZipCodeAssignedforMultipleResponse,

        /// <summary>
        /// The address, exactly as submitted, could not be found in the city, state, or Zip Code provided.
        /// </summary>
        [Display(Name = "F")]
        AddressNotFoundInNationalDirectory,

        /// <summary>
        /// Information in the firm line was determined to be a part of the address. It was moved out of the firm line and incorporated into the address line.
        /// </summary>
        [Display(Name = "G")]
        InformationInFirmLineUsedForMatching,

        /// <summary>
        /// Zip+4 information indicated this address is a building. The address as submitted does not contain an apartment/suite number.
        /// </summary>
        [Display(Name = "H")]
        MissingSecondaryNumber,

        /// <summary>
        /// More than one Zip+4 was found to satisfy the address as submitted. The submitted address did not contain sufficiently complete or correct data to determine a single Zip+4 Code.
        /// </summary>
        [Display(Name = "I")]
        InsufficientIncorrectAddressData,
        
        /// <summary>
        /// The input contained two addresses.
        /// </summary>
        [Display(Name = "J")]
        DualAddress,

        /// <summary>
        /// CASS rule does not allow a match when the cardinal point of a directional changes more than 90%.
        /// </summary>
        [Display(Name = "K")]
        MultipleResponseDueToCardinalRule,

        /// <summary>
        /// An address component was added, changed, or deleted in order to achieve a match.
        /// </summary>
        [Display(Name = "L")]
        AddressComponentChanged,

        /// <summary>
        /// Match has been converted via LACS.
        /// </summary>
        [Display(Name = "LI")]
        MatchConvertedViaLACS,

        /// <summary>
        /// The spelling of the street name was changed in order to achieve a match.
        /// </summary>
        [Display(Name = "M")]
        StreetNameChanged,

        /// <summary>
        /// The delivery address was standardized.
        /// </summary>
        [Display(Name = "N")]
        AddressStandardized,

        /// <summary>
        /// More than one Zip+4 Code was found to satisfy the address as submitted. The lowest Zip+4 addon may be used to break the tie between the records.
        /// </summary>
        [Display(Name = "O")]
        LowestZip4TieBreaker,

        /// <summary>
        /// The delivery address is matchable, but is known by another (preferred) name.
        /// </summary>
        [Display(Name = "P")]
        BetterAddressExists,

        /// <summary>
        /// Match to an address with a unique Zip Code.
        /// </summary>
        [Display(Name = "Q")]
        UniqueZipCodeMatch,

        /// <summary>
        /// The delivery address is matchable, but the EWS file indicates that an exact match will be available soon.
        /// </summary>
        [Display(Name = "R")]
        NoMatchDueToEWS,

        /// <summary>
        /// The secondary information does not match that on the national Zip+4 file. This secondary information, although present on the input address, was not valid in the range found on the national Zip+4 file.
        /// </summary>
        [Display(Name = "S")]
        IncorrectSecondaryAddress,

        /// <summary>
        /// The search resulted on a single response; however, the record matched was flagged as having magnet street syndrome. 
        /// </summary>
        [Display(Name = "T")]
        MultipleResponseDueToMagnetStreetSyndrome,

        /// <summary>
        /// The city or post office name in the submitted address is not recognized by the United States Postal Service as an official last line name (preferred city name) and is not acceptable as an alternate name.
        /// </summary>
        [Display(Name = "U")]
        UnofficialPostOfficeName,

        /// <summary>
        /// The city and state in the submitted address could not be verified as corresponding to the given 5-digit Zip Code.
        /// </summary>
        [Display(Name = "V")]
        UnverifiableCityState,

        /// <summary>
        /// The input address record contains a delivery address other than a PO BOX, General Delivery, or Postmaster with a 5-digit Zip Code that is identified as a “small town default.” The United States Postal Service does not provide street delivery for this Zip Code. The United States Postal Service requires use of a PO BOX, General Delivery, or Postmaster for delivery within this Zip Code.
        /// </summary>
        [Display(Name = "W")]
        InvalidDeliveryAddress,

        /// <summary>
        /// Default match inside a unique Zip Code.
        /// </summary>
        [Display(Name = "X")]
        UniqueZipCodeGenerated,

        /// <summary>
        /// Match made to a record with a military Zip Code.
        /// </summary>
        [Display(Name = "Y")]
        MilitaryMatch,
        
        /// <summary>
        /// The ZIPMOVE product shows which Zip+4 records have moved from one Zip Code to another. 
        /// </summary>
        [Display(Name = "Z")]
        MatchMadeUsingZIPMOVEProductData
    }
}