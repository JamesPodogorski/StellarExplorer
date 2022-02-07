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

    [Fact]
    public void TestDefaultString()
    {
        string s = default;
        if (string.Empty.Equals(s))
        {
            Console.WriteLine(string.Format("The default string is empty '{0}'", s));
        }
        if (s == null)
        {
            Console.WriteLine(string.Format("The default string is null"));
        }
    }
}