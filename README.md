# USPS.AddressApi
A simple, easy to use .NET Standard library for interacting with the USPS Address Information Api

## Summary

The goal of this project is to provide a simple, easy-to-use way to access the USPS Address Information Api set of endpoints. Use of this project, whether via source or NuGet package, 
is contingent on setting up a developer account with USPS. You can do so free of charge [here](https://www.usps.com/business/web-tools-apis/general-api-developer-guide.htm). 
Please pay special attention to the usage restrictions set forth by the terms and conditions. It is your responsibility to follow them.

This library supports the following USPS Address Information Api operations:

1. Address Validation
2. ZipCode Lookup
3. City/State Lookup

All of the above supported operations support batching of up to 5 items per request. 

## Getting Started

The easiest way to get started is to simply add the nuget package to your project via the dotnet CLI:

`dotnet add package USPS.AddressApi`

## Usage

There are two main ways to use this library. The first is the simplest and perhaps the most straight forward. You need only new up an instance of `AddressApiClient`. Example:

```csharp
// Setup userid, etc.
var opts = new AddressApiClientOptions() 
{ 
    UserId = "REPLACE_WITH_YOUR_UNIQUE_ACCOUNT_ID" 
};

// Create an instance of the api client.
var api = new AddressApiClient(opts);

// Create an address to search by.
var address = new Address()
{
    Address1 = "SUITE K",
    Address2 = "29851 Aventura",
    State = "CA",
    Zip5 = "92688"
};

// Execute
var result = await api.ValidateAddressAsync(address);
```

The second method of using the api is contractually the same as the above, but allows for the registration of an `IAddressApiClient` instance with the DI container. The following will register a `singleton` instance of `IAddressApiClient`for the lifetime of your application. 
There are different overloads to this method that should allow for most configuration scenarios. If using the `IConfiguration` overload, 
note that internally the system supports `IOptionsSnapshot` so any changes from supported configuration providers should be picked up and applied at runtime as well.

```csharp
// Register with the DI container
services.AddAddressApiClient(...);
```

### Batching

As mentioned earlier, each method supports querying of up to 5 items per request. The following is an example of how to validate multiple addresses at once.

```csharp
// Setup userid, etc.
var opts = new AddressApiClientOptions() 
{ 
    UserId = "REPLACE_WITH_YOUR_UNIQUE_ACCOUNT_ID" 
};

// Create an instance of the api client.
var api = new AddressApiClient(opts);

// Create an address to search by.
var address1 = new Address()
{
    Address1 = "SUITE K",
    Address2 = "29851 Aventura",
    State = "CA",
    Zip5 = "92688"
};

// Create second address to search by.
var address2 = new Address()
{
    Address2 = "8888 Sample Address Way",
    State = "CA",
    Zip5 = "11111"
};

// Create third address to search by.
var address3 = new Address()
{
    Address2 = "9999 Another Address Ln",
    State = "CA",
    Zip5 = "11111"
};

// Execute
var result = await api.ValidateAddressAsync(address1, address2, address3);
```
