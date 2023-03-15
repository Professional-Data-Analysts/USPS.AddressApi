using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace USPS.AddressApi
{
    internal class AddressApiConnection : IAddressApiConnection
    {
        private static IHttpClientFactory _factory;
        
        static AddressApiConnection()
        {
            var services = new ServiceCollection();
            services.AddHttpClient(nameof(AddressApiConnection));
            var provider =  services.BuildServiceProvider();
            _factory = provider.GetRequiredService<IHttpClientFactory>();
        }

        public HttpClient CreateHttpClient(Uri baseApiUri)
        {
            var client = _factory.CreateClient(nameof(AddressApiConnection));
            client.BaseAddress = baseApiUri;
            return client;
        }
    }
}