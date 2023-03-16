using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USPS.AddressApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using USPS.AddressApi.Configuration;

namespace USPS.AddressApi.Tests
{
     public class ServiceCollectionExtensionTests
    {

        [Fact]
         public void Should_Not_Resolve_Api_Service_With_Null_Argument()
         {
            // Arrange
            var services = new ServiceCollection();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => services.AddAddressApiClient(default(Action<AddressApiClientOptions>)));
         }

         [Fact]
         public void Should_Resolve_Api_Service_With_Basic_Registration()
         {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAddressApiClient(configure => { configure.UserId = "TESTUSER123"; });

            var provider = services.BuildServiceProvider();
            var api = provider.GetService<IAddressApiClient>();

            // Assert
            Assert.NotNull(api);

         }

         [Fact]
         public void Should_Resolve_Api_Service_Within_Scoped_Basic_Registration()
         {
            // Arrange
            var services = new ServiceCollection();
            IAddressApiClient? api;

            // Act
            services.AddAddressApiClient(configure => { configure.UserId = "TESTUSER123"; });

            var provider = services.BuildServiceProvider();

            using(var scope = provider.CreateScope())
            {
                 api = scope.ServiceProvider.GetService<IAddressApiClient>();
            }

            // Assert
            Assert.NotNull(api);
         }

         [Fact]
         public void Should_Not_Resolve_Api_Service_With_Null_Configuration_Argument()
         {
            // Arrange
            var services = new ServiceCollection();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => services.AddAddressApiClient(default(IConfiguration)));
         }


         [Fact]
         public void Should_Resolve_Api_Service_With_Config_Based_Registration()
         {
            // Arrange
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder();
            var dict = new Dictionary<string, string?>()
            {
                {$"{AddressApiClientOptions.CONFIGURATION_SECTION_NAME}:UserId", "TESTUSER123"}
            };
            builder.AddInMemoryCollection(dict);
            var config = builder.Build();

            // Act
            services.AddAddressApiClient(config);

            var provider = services.BuildServiceProvider();
            var api = provider.GetService<IAddressApiClient>();

            // Assert
            Assert.NotNull(api);
         }

         [Fact]
         public void Should_Resolve_Api_Service_Within_Scoped_Config_Based_Registration()
         {
            // Arrange
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder();
            var dict = new Dictionary<string, string?>()
            {
                {"AddressDex:UserId", "TESTUSER123"}
            };
            builder.AddInMemoryCollection(dict);
            var config = builder.Build();

            // Act
            services.AddAddressApiClient(config);

            var provider = services.BuildServiceProvider();
            IAddressApiClient? api;

            using(var scope = provider.CreateScope())
            {
                 api = scope.ServiceProvider.GetService<IAddressApiClient>();
            }

            // Assert
            Assert.NotNull(api);
         }
    }
}