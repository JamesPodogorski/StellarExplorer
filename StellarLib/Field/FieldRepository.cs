using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StellarLib;

public class FieldRepository : Repository<Field>, IFieldRepository
{
    private ILogger<FieldRepository> logger;
    public FieldRepository(ILoggerFactory loggerFactory,
                            IHttpClientFactory httpClientFactory, IOptions<StellarServiceOptions> options,
                            ITokenService tokenService)
                        : base(loggerFactory, httpClientFactory, options, tokenService)
    {
        Logger.LogInformation("Field Repository doing its thing.");
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

        Field field = null;
        string[] pathParam = new string[] {
            "farmers",
            "JamesFarmer1",
            "farms",
            id
        };
        var queryParam = new Dictionary<string, string>() {
                {  "api-version", options.apiVersion }
            };

        var uriBuilder = new UriBuilder(string.Format(@"{0}://{1}", "https", options.host));
        uriBuilder.Path(pathParam);
        uriBuilder.Query(queryParam);

        var result = await SendApiCallResults(uriBuilder, HttpMethod.Get, null);
        if (!(result.code == HttpStatusCode.OK || result.code == HttpStatusCode.NotFound))
        {
            Logger.LogError(string.Format("Error retrieving Farmer {0}; {1}"), id, result.code);
        }
        else if (result.code == HttpStatusCode.OK)
        {
            field = JsonHelper.Deserialize<Field>(result.resp);
        }
        else
        {
        }
        return field;
    }

    // TODO: Should this be defined in the interface or inherated.
    // TODO: Async correction in other methods....  they are not async.
    public async override Task<IEnumerable<Field>> GetAll()
    {
        // TODO: Apparently this returns all the stuff, i.e. xxx farms.  What we need to do is create the Farm object....
        var myList = getData("fields");
        var l = new List<Field>();

        // As a test only.... to probe
        string testJson = JsonHelper.Serialize<IList<object>>(myList);
        File.WriteAllText(string.Format("../../../{0}.json", "testJson"), testJson);

        foreach (object o in myList)
        {
            string jsonO = JsonHelper.Serialize<object>(o);
            var field = JsonHelper.Deserialize<Field>(jsonO);
            l.Add(field);
        }

        string json = JsonHelper.Serialize<IList<Field>>(l);
        File.WriteAllText(string.Format("../../../{0}.json", "fields"), json);
        return await Task.FromResult<IEnumerable<Field>>(l);
    }

    public async Task<IEnumerable<Field>> GetAllFarms(string farmerId)
    {
        string operation = string.Format(@"farmers/{0}/farms", farmerId);
        var myList = getData(operation);
        var l = new List<Field>();
        foreach (object o in myList)
        {
            string jsonO = JsonHelper.Serialize<object>(o);
            var farm = JsonHelper.Deserialize<Field>(jsonO);
            l.Add(farm);
        }

        string json = JsonHelper.Serialize<IList<Field>>(l);
        File.WriteAllText(string.Format("../../../{0}.json", "farms"), json);
        return await Task.FromResult<IEnumerable<Field>>(l);
    }
}
