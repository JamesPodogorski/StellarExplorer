using System;
using System.Net.Http;
using System.Net;
using System.Text;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace StellarLib;

public class RestHelper
{
    public struct Result { public HttpStatusCode code; public string resp; }

    private static readonly string _patch_Content_Type = @"application/merge-patch+json";
    private static readonly string _get_Content_Type = @"application/json";
    private static readonly string _post_Content_Type = @"application/json";

    public readonly Dictionary<string, string> _contentType = new Dictionary<string, string>()
        {
            { "PATCH", _patch_Content_Type },
            { "GET", _get_Content_Type },
            { "POST", _post_Content_Type }
        };

    // TODO: figure out _host and authclient and _scheme below
    private static HttpClient authClient = new HttpClient();
    private string _host = string.Empty;
    private string _scheme = "https";

    public async Task<string> GetAccessToken()
    {
        throw new NotImplementedException();
        // if (!_access_code.Equals(string.Empty))
        //     return await Task.FromResult<string>(_access_code);

        // Dictionary<string, string> data = new Dictionary<string, string>()
        //     {
        //         { "grant_type", "client_credentials" },
        //         { "client_id", this._client_id },
        //         { "resource", this._resource },
        //         { "client_secret", this._secret }
        //     };

        // var form = new FormUrlEncodedContent(data);
        // string url = this._login_url;
        // try
        // {
        //     HttpResponseMessage resp = await authClient.PostAsync(url, form);
        //     resp.EnsureSuccessStatusCode();
        //     string responseBody = await resp.Content.ReadAsStringAsync();
        //     var doc = JsonDocument.Parse(responseBody, default(JsonDocumentOptions));
        //     _access_code = doc.RootElement.GetProperty("access_token").GetString();
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e.Message);
        // }
        // // var v = await Task.FromResult(string.Empty);
        // return _access_code;
    }
    public async Task<Result> SendApiCall(Dictionary<string, string> pathParameters,
                          Dictionary<string, string> queryParameters,
                          string jsonBody,
                          HttpMethod method)
    {
        Result res;
        HttpStatusCode code = default(HttpStatusCode);
        string access_token = await this.GetAccessToken();

        var builder = new UriBuilder()
        {
            Host = this._host,
            Scheme = this._scheme,
        };
        builder.Path(pathParameters);
        builder.Query(queryParameters);

        HttpRequestMessage req = new HttpRequestMessage()
        {
            Method = method,
            Content = new StringContent(jsonBody, Encoding.UTF8,
                                        _contentType.GetValueOrDefault(method.ToString())),
            RequestUri = builder.Uri
        };
        req.Headers.Add("Authorization", string.Format("Bearer {0}", access_token));
        try
        {
            var resp = await authClient.SendAsync(req);
            string responseBody = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);
            code = resp.StatusCode;
            res = new Result() { code = code, resp = responseBody };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw e;
        }
        return res;
    }

    public async Task<Result> SendApiCallResults(UriBuilder builder,
                                                string jsonBody,
                                                HttpMethod method)
    {
        Result res;
        HttpStatusCode code = default(HttpStatusCode);
        string access_token = await this.GetAccessToken();

        HttpRequestMessage req = null;
        if (string.Empty.Equals(jsonBody))
        {
            req = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = builder.Uri
            };
        }
        else
        {
            req = new HttpRequestMessage()
            {
                Method = method,
                Content = new StringContent(jsonBody, Encoding.UTF8,
                                            _contentType.GetValueOrDefault(method.ToString())),
                RequestUri = builder.Uri
            };
        }
        req.Headers.Add("Authorization", string.Format("Bearer {0}", access_token));
        try
        {
            var resp = await authClient.SendAsync(req);
            string responseBody = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);
            code = resp.StatusCode;
            res = new Result() { code = code, resp = responseBody };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw e;
        }
        return res;
    }

    public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    {
        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}