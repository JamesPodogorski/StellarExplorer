namespace StellarLib;

using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

public class StellarService : IStellarService
{
    private HttpClient? _client;
    private ILogger _logger;
    public ILoggerFactory factory { get; set; }

    public StellarService()
    {
        _client = new HttpClient();
    }
    public StellarService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("stellar");
    }

    public StellarService(IHttpClientFactory factory, TokenServiceOptions option)
    {
        _client = factory.CreateClient("stellar");
    }

    public StellarService(IHttpClientFactory factory, IOptions<TokenServiceOptions> option)
    {
        _client = factory.CreateClient("stellar");
    }

    // public StellarService(IHttpClientFactory factory, IOptions<TokenServiceOptions> option, ILoggerFactory loggerFactory)
    // {
    //     _client = factory.CreateClient("stellar");
    //     var logger = loggerFactory.CreateLogger("asdf");
    //     logger.LogInformation(1, "hellow world");
    //     Console.WriteLine("ok");
    // }

    // public StellarService(HttpClient client)
    // {
    //     _client = client;
    // }

    public string GetSomething()
    {
        return "hello";
    }
}