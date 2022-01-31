using StellarLib;
using System;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.IO;

namespace tests;

public class TestHelper
{
    public readonly static string StellarConfigSection = "StellarServiceOptions";
    public readonly static string TokenOptionSectionName = "TokenServiceOptions";
    public readonly static string TokenEnvPrefix = "FB";
    public readonly static string ClientId_Env = string.Format("%s_client_id", TokenEnvPrefix);
    public readonly static string ApplicationSecret_Env = string.Format("%s_application_secret", TokenEnvPrefix);
    public static IAsyncPolicy<HttpResponseMessage> GetStandardRetryPolicy()
    {
        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    // TODO: Verify this works for all types...
    public static T GetServiceOptions<T>(string sectionName)
    {
        var configBuilder = new ConfigurationBuilder()
                        // .AddEnvironmentVariables(TokenEnvPrefix)
                        .AddJsonFile("appsettings.json")
                        .Build();
        var section = configBuilder.GetSection(sectionName);
        return section.Get<T>();
    }

    IAsyncPolicy<HttpResponseMessage> GetPolicy()
    {
        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}