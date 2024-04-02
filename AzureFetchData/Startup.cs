using System;
using AzureFetchData.Libraries;
using AzureFetchData.Libraries.Interfaces;
using AzureFetchData.Libraries.Services;
using AzureFetchData.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Refit;

[assembly: FunctionsStartup(typeof(AzureFetchData.Startup))]
namespace AzureFetchData;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();
        builder.Services.AddSingleton<ILogTableService, LogTableService>();
        builder.Services.AddSingleton<IValidator<HttpRequest>, DateValidator>();
        builder.Services.AddSingleton<IPayloadBlobService, PayloadBlobService>();

        builder.Services.AddRefitClient<IPublicApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.publicapis.org"));
    }
}