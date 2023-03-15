using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace USPS.AddressApi.Models
{
    /// <summary>
    /// Represents the response from the Address Api.
    /// </summary>
    /// <typeparam name="TResult">The item-level result type.</typeparam>
    /// <typeparam name="TRequest">The item-level request type.</typeparam>
    public abstract class ResponseBase<TResult, TRequest> where TResult : ResultBase<TRequest>
    {
        /// <summary>
        /// The list of individual results from the request.
        /// </summary>
        public List<TResult> Results { get; set; }

        /// <summary>
        /// Represents a top-level error from the Address Api. If set, this typically indicates a malformed request or system error.
        /// </summary>
        public AddressApiError Error { get; set; }

        /// <summary>
        /// Creates a new instance of ResponseBase.
        /// </summary>
        public ResponseBase()
        {
            Results = new List<TResult>();
        }

        internal ResponseBase(XDocument responseDocument, string resultItemName, params TRequest[] originalItems) 
            : this()
        {
            if(AddressApiError.TryParse(responseDocument.Root, out var error))
            {
                Error = error;
            }
            else
            {
                foreach(var element in responseDocument.Root.Elements(resultItemName).OrderBy(x => x.Attribute(Constants.ID_ATTRIBUTE_NAME).Value))
                {
                    if(string.IsNullOrWhiteSpace(element.Attribute(Constants.ID_ATTRIBUTE_NAME)?.Value) || !int.TryParse(element.Attribute(Constants.ID_ATTRIBUTE_NAME)?.Value, out var trackingId))
                    {
                        throw new InvalidOperationException($"Invalid tracking identifier found. Unable to associate the result with the original. Identifier was: {element.Attribute(Constants.ID_ATTRIBUTE_NAME)?.Value}");
                    }

                    var result = Activator.CreateInstance(
                        typeof(TResult), 
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                        null, 
                        new object[] {
                            element, 
                            originalItems[trackingId]
                        }, System.Globalization.CultureInfo.InvariantCulture) as TResult;

                    Results.Add(result);
                }
            }
        }
    }
}