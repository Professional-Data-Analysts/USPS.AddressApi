using System.Xml.Linq;
using USPS.AddressApi.Enumerations;
using USPS.AddressApi.Extensions;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the item-level result from the USPS Address Api.
    /// </summary>
    public abstract class ResultBase<T>
    {
        /// <summary>
        /// The exact, original object supplied to the original request.
        /// </summary>
        public T Original { get; set; }

        /// <summary>
        /// If lookup request is successful, indicates the type of lookup result.
        /// </summary>
        public ApiResultType ResultType { get; set; }

        /// <summary>
        /// Additional message sent back from the USPS Api for the given result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new instance of ResultBase.
        /// </summary>
        public ResultBase() { }

        internal ResultBase(XElement element, T original)
        {
            Original = original;
            if(AddressApiError.TryParse(element, out var error))
            {
                if(element.Element(Constants.ERROR_ELEMENT_NAME).TryGetEnumFromChildElement<ApiResultType>(nameof(AddressApiError.Description), out var resultType))
                {
                    ResultType = resultType;
                    Message = error.Description;
                }
                else
                {
                    Message = error.Description;
                    ResultType = ApiResultType.UnknownResult;
                }
            }
            else
            {
                ResultType = ApiResultType.Match;
            }
        }
    }
}