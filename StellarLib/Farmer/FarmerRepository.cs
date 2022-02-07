using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StellarLib;

public class FarmerRepository : Repository<Field>, IFarmerRepository
{
    private ILogger<FarmerRepository> logger;
    public FarmerRepository(ILoggerFactory loggerFactory,
                            IHttpClientFactory httpClientFactory, IOptions<StellarServiceOptions> options,
                            ITokenService tokenService)
                        : base(loggerFactory, httpClientFactory, options, tokenService)
    {
        Logger.LogInformation("FarmRepository doing its thing.");
    }

    public async override Task<Field> GetById(string id)
    {
        // var pathParam = new Dictionary<string, string>() {
        //         {  "farmers", id },
        //         {  "tillage-data", tillage.identity },
        //     };
        // var queryParam = new Dictionary<string, string>() {
        //         {  "api-version", _apiVersion }
        //     };

        Field farmer = null;
        var pathParam = new Dictionary<string, string>() {
                {  "farmers", id }
            };
        var queryParam = new Dictionary<string, string>() {
                {  "api-version", options.apiVersion }
            };

        var uriBuilder = new UriBuilder(string.Format(@"{0}://{1}", "https", options.host));
        uriBuilder.Path(pathParam);
        uriBuilder.Query(queryParam);

        var result = await SendApiCallResults(uriBuilder, HttpMethod.Get);
        if (!(result.code == HttpStatusCode.OK || result.code == HttpStatusCode.NotFound))
        {
            Logger.LogError(string.Format("Error retrieving Farmer {0}; {1}"), id, result.code);
        }
        else if (result.code == HttpStatusCode.OK)
        {
            farmer = new Field { name = "hello" };
        }
        else
        {
            farmer = null;
        }
        return farmer;
    }

    // TODO: Should this be defined in the interface or inherated.
    // TODO: Async correction in other methods....  they are not async.
    public async override Task<IEnumerable<Field>> GetAll(CancellationToken token = default)
    {
        // var pathParam = new Dictionary<string, string>() {
        //         {  "farmers", id },
        //         {  "tillage-data", tillage.identity },
        //     };
        // var queryParam = new Dictionary<string, string>() {
        //         {  "api-version", _apiVersion }
        //     };

        // string[] pathParam = new string[] { "farmers" };
        // var queryParam = new Dictionary<string, string>() {
        //         {  "api-version", options.apiVersion }
        //     };

        // var uriBuilder = new UriBuilder(string.Format(@"{0}://{1}", "https", options.host));
        // uriBuilder.Path(pathParam);
        // uriBuilder.Query(queryParam);

        // var result = await SendApiCallResults(uriBuilder, HttpMethod.Get, null);
        // if (!(result.code == HttpStatusCode.OK || result.code == HttpStatusCode.NotFound))
        // {
        //     Logger.LogError(string.Format("Error retrieving Farmers; {1}"), result.code);
        // }
        // return result.resp;

        // TODO: Apparently this returns all the stuff, i.e. 13 farmers.  What we need to do is create the Farmer object....
        IList<object> myList;
        try
        {
            myList = getData("farmers");
        }
        catch (Exception ex1)
        {
            logger.LogError(ex1, string.Empty, null);
            throw new ApplicationException("Exception getting farmers");
        }
        var l = new List<Field>();

        // As a test only.... to probe
        string testJson = JsonHelper.Serialize<IList<object>>(myList);
        // File.WriteAllText(string.Format("../../../{0}.json", "testJson"), testJson);

        foreach (object o in myList)
        {
            string jsonO = JsonHelper.Serialize<object>(o);
            var farmer = JsonHelper.Deserialize<Field>(jsonO);
            l.Add(farmer);
        }
        string farmersJson = JsonHelper.Serialize<IList<Field>>(l);
        // File.WriteAllText(string.Format("{0}.json", "farmers"), farmersJson);

        string json = JsonHelper.Serialize<IList<Field>>(l);
        // File.WriteAllText(string.Format("../../../{0}.json", "farmers"), json);
        LogFamers(l);
        return await Task.FromResult<IEnumerable<Field>>(l);
    }

    private void LogFamers(IEnumerable<Field> farmers)
    {
        foreach (Field farmer in farmers)
            Logger.LogInformation("Farmer '{0}.", farmer.name);
    }

    public async Task<IEnumerable<Field>> GetAllWithoutCall()
    {
        return await Task.FromResult<IEnumerable<Field>>(new List<Field>()
        {
            new Field() { name = "john "},
            new Field() { name = "james" }
        });
    }
}