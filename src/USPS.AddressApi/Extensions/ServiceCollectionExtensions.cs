using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using USPS.AddressApi.Configuration;

namespace USPS.AddressApi.Extensions
{
    /// <summary>
    /// Extension methods for setting up an <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a singleton instance of <see cref="IAddressApiClient"/> with the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register against.</param>
        /// <param name="configure">The supplied configuration</param>
        public static IServiceCollection AddAddressApiClient(this IServiceCollection services, Action<AddressApiClientOptions> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.Configure(configure);
            services.AddSingleton<IAddressApiClient, AddressApiClient>((provider) =>
            {
                var options = provider.GetRequiredService<IOptions<AddressApiClientOptions>>();
                var logger = provider.GetService<ILogger<AddressApiClient>>();
                return new AddressApiClient(new AddressApiConnection(), options?.Value, logger);
            });

            return services;
        }

        /// <summary>
        /// Registers a singleton instance of <see cref="IAddressApiClient"/> with the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register against.</param>
        /// <param name="configuration">the <see cref="IConfiguration"/> in which to pull configuration data from</param>
        public static IServiceCollection AddAddressApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<AddressApiClientOptions>(configuration.GetSection(AddressApiClientOptions.CONFIGURATION_SECTION_NAME));
            services.AddSingleton<IAddressApiClient, AddressApiClient>((provider) =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<AddressApiClientOptions>>();
                var logger = provider.GetService<ILogger<AddressApiClient>>();
                return new AddressApiClient(new AddressApiConnection(), options, logger);
            });

            return services;
        }
    }
}