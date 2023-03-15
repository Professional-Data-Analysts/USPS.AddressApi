using System;
using System.Net.Http;

namespace USPS.AddressApi
{
    internal interface IAddressApiConnection
    {
        HttpClient CreateHttpClient(Uri baseApiUri);
    }
}