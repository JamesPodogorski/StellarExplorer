using Xunit;
using StellarLib;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace tests;

public class UnitTestCrop
{
    [Fact]
    public async void TestGetAllCrops()
    {
        //* Config Options
        var tokenServiceOptions = TestHelper.GetServiceOptions<TokenServiceOptions>(TestHelper.TokenOptionSectionName);
        var stellarServiceOptions = TestHelper.GetServiceOptions<StellarServiceOptions>(TestHelper.StellarConfigSection);

        //* Service Collection
        var serviceCollection = new ServiceCollection().AddLogging(options =>
        {
            options.AddConsole();
        });

        //* Services
        serviceCollection.AddTransient<ICropRepository, CropRepository>();
        serviceCollection.AddSingleton<ITokenService, TokenService>();

        //* Htpp Clients
        // TODO: add retry policies to http clients
        // TODO: fix the BaseAddress for our Stellar Service
        serviceCollection.AddHttpClient(stellarServiceOptions.httpClientName, client =>
        {
            // client.BaseAddress = new Uri("https://www.asdf.com");
        }).AddPolicyHandler(policy => RestHelper.GetPolicy());

        serviceCollection.AddHttpClient(tokenServiceOptions.httpClientName, client =>
        {
        }).AddPolicyHandler(policy => RestHelper.GetPolicy());

        //* Service Config
        serviceCollection.Configure<TokenServiceOptions>(options =>
        {
            options.applicationSecret = tokenServiceOptions.applicationSecret;
            options.clientId = tokenServiceOptions.clientId;
            options.httpClientName = tokenServiceOptions.httpClientName;
            options.resource = tokenServiceOptions.resource;
            options.tenantId = tokenServiceOptions.tenantId;
        });

        serviceCollection.Configure<StellarServiceOptions>(options =>
        {
            options.apiVersion = stellarServiceOptions.apiVersion;
            options.grant_type = stellarServiceOptions.grant_type;
            options.host = stellarServiceOptions.host;
            options.resource = stellarServiceOptions.resource;
            options.tenant_id = stellarServiceOptions.tenant_id;
            options.httpClientName = stellarServiceOptions.host;
        });

        //* Get instance and test service call
        var provider = serviceCollection.BuildServiceProvider();
        var cropRepository = provider.GetService<ICropRepository>();
        var resp = await cropRepository.GetAll();

        // TODO: Works, refine what you want to test and verify before putting it into Blazor.
        // TODO: Maybe get Farms and Fields working, then abstract out.
        var ids = from o in resp
                    select o.id;

        foreach (string id in ids)
        {
            Console.WriteLine(id);
        }
    }
}