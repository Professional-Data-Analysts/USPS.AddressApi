using System.ComponentModel.DataAnnotations;

namespace USPS.AddressApi.Enumerations
{
    /// <summary>
    /// The DPV Confirmation Indicator is the primary method used by the USPS to determine whether an address was considered deliverable or undeliverable.
    /// </summary>
    public enum DPVConfirmationCodeType
    {
        /// <summary>
        /// Address was DPV confirmed for both primary and (if present) secondary numbers.
        /// </summary>
        [Display(Name = "Y")]
        DPVBothConfirmed,

        /// <summary>
        /// Address was DPV confirmed for the primary number only, and the secondary number information was missing.
        /// </summary>
        [Display(Name = "D")]
        DPVConfirmedPrimaryMissingSecondary,

        /// <summary>
        /// Address was DPV confirmed for the primary number only, and the secondary number information was present by not confirmed.
        /// </summary>
        [Display(Name = "S")]
        DPVConfirmedPrimaryNotConfirmedSecondary,

        /// <summary>
        /// Both primary and (if present) secondary number information failed to DPV confirm.
        /// </summary>
        [Display(Name = "N")]
        DPVBothNotConfirmed
    }
}