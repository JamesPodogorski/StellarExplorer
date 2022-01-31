using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace StellarLib;

public class TokenService : ITokenService
{
    private string accessCode = string.Empty;
    private HttpClient? httpClient;
    private TokenServiceOptions options;
    private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    private ILogger logger;
    private string baseFormat = @"{0}://login.microsoftonline.com/{1}/oauth2/token";
    private string url = string.Empty;

    // public TokenService()
    // {
    //     _options.ApplicationSecret = "asdf";
    // }
    // public TokenService(IHttpClientFactory factory, IOptions<TokenServiceOptions> options)
    // {
    //     httpClient = factory.CreateClient("asdf");
    //     _options = options.Value;
    // }

    public TokenService(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory, IOptions<TokenServiceOptions> options)
    {
        logger = loggerFactory.CreateLogger<TokenService>();
        this.options = options.Value;
        httpClient = httpClientFactory.CreateClient(this.options.httpClientName);
        url = string.Format(baseFormat, "https", this.options.tenantId); 
    }


    #region Public

    public async Task<string> GetAccessToken()
    {
        try
        {
            semaphoreSlim.Wait();

            if (HasExpired() || IsEmptyToken())
            {
                accessCode = await GetTokenFromService();
            }
        }
        catch (Exception ex1)
        {
        logger.LogError(ex1.Message);
        }
        finally
        {
            semaphoreSlim.Release();
        }

        return accessCode;
    }

    #endregion Public

    #region Private

    // TODO: Implement
    private bool HasExpired()
    {
        return false;
    }

    private bool IsEmptyToken()
    {
        return accessCode.Equals(string.Empty);
    }

    private async Task<string> GetTokenFromService()
    {
        if (!accessCode.Equals(string.Empty))
            return await Task.FromResult<string>(accessCode);

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "grant_type", "client_credentials" },
            { "client_id", options.clientId },
            { "resource", options.resource },
            { "client_secret", options.applicationSecret }
        };

        var form = new FormUrlEncodedContent(data);

        try
        {
            HttpResponseMessage resp = await httpClient.PostAsync(url, form);
            resp.EnsureSuccessStatusCode();
            string responseBody = await resp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(responseBody, default(JsonDocumentOptions));
            accessCode = doc.RootElement.GetProperty("access_token").ToString();
        }
        catch (Exception ex1)
        {
            logger.LogError(ex1, "Cannot retrieve access token");
        }
        return accessCode;
    }
    #endregion Private
}