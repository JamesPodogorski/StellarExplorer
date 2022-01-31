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

public class UnitTest1
{
    [Fact]
    public async void TestGetAllFarmers()
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
        serviceCollection.AddTransient<IFarmerRepository, FarmerRepository>();
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
        var farmerRepository = provider.GetService<IFarmerRepository>();
        var resp = await farmerRepository.GetAll();

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

        foreach (string name in names)
        {
            Console.WriteLine(name);
        }
    }

    [Fact]
    public async void TestGetFarmer()
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
        serviceCollection.AddTransient<IFarmerRepository, FarmerRepository>();
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
        var farmerRepository = provider.GetService<IFarmerRepository>();
        string id = "Farmer-y";
        var farmer = await farmerRepository.GetById(id);

        Console.WriteLine(farmer is not null ? farmer.name : String.Format("Farmer '{0}' not found", id));
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

    [Fact]
    public async Task TokenServiceTest()
    {
        var tokenServiceOptions = TestHelper.GetServiceOptions<TokenServiceOptions>(TestHelper.TokenOptionSectionName);

        var serviceCollection = new ServiceCollection().AddLogging(options =>
        {
            options.AddConsole();
        });

        serviceCollection.AddHttpClient(tokenServiceOptions.httpClientName, client =>
        {
        });

        serviceCollection.Configure<TokenServiceOptions>(options =>
        {
            options.applicationSecret = tokenServiceOptions.applicationSecret;
            options.clientId = tokenServiceOptions.clientId;
            options.httpClientName = tokenServiceOptions.httpClientName;
            options.resource = tokenServiceOptions.resource;
            options.tenantId = tokenServiceOptions.tenantId;
        });

        serviceCollection.AddTransient<ITokenService, TokenService>();

        var provider = serviceCollection.BuildServiceProvider();
        var tokenService = provider.GetService<ITokenService>();
        var accessToken = await tokenService.GetAccessToken();
        Console.WriteLine(accessToken);
    }


    [Fact]
    public void TestConfigInjection()
    {
        StellarServiceOptions stellarOptions = null;
        TokenServiceOptions tokenServiceOptions = null;
        // getConfig();

        var scx = new ServiceCollection().AddLogging(options =>
        {
            options.AddConsole();
        });

        scx.AddSingleton<AppLogging>();
        var pcx = scx.BuildServiceProvider();
        var acx = pcx.GetService<AppLogging>();
        var sc = new ServiceCollection().AddTransient<IStellarService, StellarService>();

        configLogging();

        sc.Configure<TokenServiceOptions>(options => options.applicationSecret = "hel");
        sc.AddHttpClient("stellar", client =>
        {
            client.BaseAddress = new Uri("https://www.asdf.com");
        });
        var p = sc.BuildServiceProvider();
        var s = p.GetService<IStellarService>();
        // p.GetRequiredService
        s.GetSomething();

        var sc2 = new ServiceCollection().AddSingleton<ITokenService, TokenService>();
        sc2.Configure<TokenServiceOptions>(options => options.applicationSecret = "hel");
        sc2.AddHttpClient("token", client =>
        {
            client.BaseAddress = new Uri("https://www.asdf.com");
        });
        var p2 = sc2.BuildServiceProvider();
        var s2 = p2.GetService<ITokenService>();
        s2.GetAccessToken();

        void getConfig()
        {
            var cm = new ConfigurationManager();
            cm.AddJsonFile("appsettings.json");
            cm.AddEnvironmentVariables(TestHelper.TokenEnvPrefix);
            var section = cm.GetSection(TestHelper.StellarConfigSection);
            stellarOptions = section.Get<StellarServiceOptions>();
            tokenServiceOptions = new TokenServiceOptions()
            {
                applicationSecret = cm.GetValue<string>(TestHelper.ApplicationSecret_Env),
                clientId = cm.GetValue<string>(TestHelper.ClientId_Env)
            };
        }

        void configLogging()
        {
            sc.AddLogging(config =>
            {
                config.AddSimpleConsole();
            });
        }
        //  var serviceProvider = new ServiceCollection()
        //             .AddLogging()
        //             .AddSingleton<IFooService, FooService>()
        //             .AddSingleton<IBarService, BarService>()
        //             .BuildServiceProvider();

        //         //configure console logging
        //         serviceProvider
        //             .GetService<ILoggerFactory>()
        //             .AddConsole(LogLevel.Debug);

        //         var logger = serviceProvider.GetService<ILoggerFactory>()
        //             .CreateLogger<Program>();
        //         logger.LogDebug("Starting application");

        //         //do the actual work here
        //         var bar = serviceProvider.GetService<IBarService>();
        //         bar.DoSomeRealWork();

        //         logger.LogDebug("All done!");
    }
}