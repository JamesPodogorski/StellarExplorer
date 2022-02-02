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

public class UnitFarms
{
    [Fact]
    public async void TestGetAllFarms()
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
        serviceCollection.AddTransient<IFarmRepository, FarmRepository>();
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
        var farmRepository = provider.GetService<IFarmRepository>();
        var resp = await farmRepository.GetAll();

        // TODO: Works, refine what you want to test and verify before putting it into Blazor.
        // TODO: Maybe get Farms and Fields working, then abstract out.
        var names = from o in resp
                    select o.name;

        foreach (string name in names)
        {
            Console.WriteLine(name);
        }
    }


    [Fact]
    public async void TestGetAllFarmerFarms()
    {
        // !This is a Farmer pulled from Stellar.
        string farmerId = "JamesFarmer1";
        //* Config Options
        var tokenServiceOptions = TestHelper.GetServiceOptions<TokenServiceOptions>(TestHelper.TokenOptionSectionName);
        var stellarServiceOptions = TestHelper.GetServiceOptions<StellarServiceOptions>(TestHelper.StellarConfigSection);

        //* Service Collection
        var serviceCollection = new ServiceCollection().AddLogging(options =>
        {
            options.AddConsole();
        });

        //* Services
        serviceCollection.AddTransient<IFarmRepository, FarmRepository>();
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
        var farmRepository = provider.GetService<IFarmRepository>();
        var resp = await farmRepository.GetAllFarms(farmerId);

        // TODO: Works, refine what you want to test and verify before putting it into Blazor.
        // TODO: Maybe get Farms and Fields working, then abstract out.
        var names = from o in resp
                    select o.name;

        var something = from o in resp
                        select new { o.name, o.description };


        foreach (var s in something)
        {
            Console.WriteLine(s.name);
        }
    }

    [Fact]
    public async Task TestGetFarm()
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
        serviceCollection.AddTransient<IFarmRepository, FarmRepository>();
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
        var farmRepository = provider.GetService<IFarmRepository>();
        string id = "ChickenFarm";
        var farm = await farmRepository.GetById(id);

        Console.WriteLine(farm is not null ? farm.name : String.Format("Farm '{0}' not found", id));
    }
}