using System;
using System.Net.Http;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StellarLib;

// TODO: clean up sections, move and region stuff

public class Repository<T> : IRepository<T> where T : FarmHierarchyBase
{

    protected ILoggerFactory loggerFactory;
    private ILogger<T> ? logger;
    protected IHttpClientFactory httpClientFactory;
    // TODO: a better name for options
    protected StellarServiceOptions options;
    protected struct Result { public HttpStatusCode code; public string resp; }
    protected static readonly string _patch_Content_Type = @"application/merge-patch+json";
    protected static readonly string _get_Content_Type = @"application/json";
    protected static readonly string _post_Content_Type = @"application/json";

    protected readonly Dictionary<string, string> _contentType = new Dictionary<string, string>()
        {
            { "PATCH", _patch_Content_Type },
            { "GET", _get_Content_Type },
            { "POST", _post_Content_Type }
        };

    // TODO: Make this into a config.
    protected string httpClientName = "stellar";
    protected ITokenService tokenService;

    // TODO: figure out _host and authclient and _scheme below
    // TODO: figure out httpclient and factory
    protected  HttpClient httpClient;
    private string _host = string.Empty;
    private string _scheme = "https";

    #region Constructors
    private Repository()
    {
        // var client = httpClientFactory.CreateClient("asdf");
        // loggerFactory = null;
    }

    protected Repository(ILoggerFactory loggerFactory, 
                        IHttpClientFactory httpClientFactory, 
                        IOptions<StellarServiceOptions> options,
                        ITokenService tokenService)
    {
        this.options = options.Value;
        this.httpClient = httpClientFactory.CreateClient(this.options.httpClientName);
        this.loggerFactory = loggerFactory;
        this.tokenService = tokenService;
    }

    #endregion Constructors

    #region Properties

    protected ILogger<T> Logger
    {
        get 
        {
            if (logger == null)
            {
                Logger = loggerFactory.CreateLogger<T>();
            }
            return logger;
        }
        private set => logger = value;
    }

    #endregion Properties

    // TODO: check this.... should it be protected??
    protected async Task<string> GetAccessToken()
    {
        return await tokenService.GetAccessToken();
    }

    protected async Task<Result> SendApiCallResults(UriBuilder builder,
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
            // var resp = await httpClient.SendAsync(req);
            var tt = httpClient.SendAsync(req);
            tt.Wait();
            var resp = tt.Result;
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

    #region IRepository methods
    public virtual async Task<T> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<IEnumerable<T>> GetAll(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    protected virtual HttpClient CreateHttpClient()
    {
        // return httpClientFactory.CreateClient(httpClientName);
        return new HttpClient();
    }

      protected async Task<Result> SendApiCallResults(UriBuilder builder,
                                                  HttpMethod method,
                                                  string? jsonBody = default,
                                                  CancellationToken token = default)
    {
        Result res;
        HttpStatusCode code = default(HttpStatusCode);
        string access_token = await GetAccessToken();

        HttpRequestMessage req = null;
        if (jsonBody is null)
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
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);
            var ct = cts.Token;
            ct = CancellationToken.None;
            var tt = httpClient.SendAsync(req, ct);
            tt.Wait();
            var resp = tt.Result;
            // var resp = await httpClient.SendAsync(req, ct);
            string responseBody = await resp.Content.ReadAsStringAsync(ct);
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
   
    #region Follow Link
    protected IList<object> getData(string operationType)
    {
        bool cont = true;
        bool first = true;
        OperationsResult hnl = null;
        IList<object> dataList = new List<object>();

        var t1 = firstCall(operationType);
        var res = t1.Result;

        while (res.code == HttpStatusCode.OK && cont)
        {
            if (!first)
            {
                var t2 = followNextLink(hnl.nextLink);
                res = t2.Result;
            }
            hnl = addData(res.resp, dataList);
            cont = shouldContinue(hnl);
            first = false;
        }
        return dataList;
    }

    // TODO: figure this out, i.e param is 'farmers' or 'planting-data' or 'tillage-data'
    private async Task<Result> firstCall(string param)
    {
        var pathParam = new string[1] { param };
        var queryParam = new Dictionary<string, string>() {
                {  "api-version", options.apiVersion }
                };

        // TODO: This will repeat, i.e. should be part of the base Repository<T> etc...
        var uriBuilder = new UriBuilder(string.Format(@"{0}://{1}", "https", options.host));
        uriBuilder.Path(pathParam);
        uriBuilder.Query(queryParam);

        return await SendApiCallResults(uriBuilder, HttpMethod.Get);
    }

    private async Task<Result> followNextLink(string nextLink)
    {
        UriBuilder builder = new UriBuilder(nextLink);
        return await SendApiCallResults(builder, HttpMethod.Get);
    }

    private OperationsResult addData(string resp, IList<object> dataList)
    {
        OperationsResult hnl = JsonHelper.Deserialize<OperationsResult>(resp);
        foreach (object o in hnl.value ?? Enumerable.Empty<object>())
        {
            dataList.Add(o);
        }
        return hnl;
    }

    private bool shouldContinue(OperationsResult hnl)
    {
        return !(hnl.nextLink == null || hnl.nextLink == string.Empty);
    }

    #endregion Follow Link

    #endregion IRepository methods
}