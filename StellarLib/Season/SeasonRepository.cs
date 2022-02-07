using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StellarLib;

public class SeasonRepository : Repository<Season>, ISeasonRepository
{
    private ILogger<SeasonRepository> logger;
    public SeasonRepository(ILoggerFactory loggerFactory,
                            IHttpClientFactory httpClientFactory, IOptions<StellarServiceOptions> options,
                            ITokenService tokenService)
                        : base(loggerFactory, httpClientFactory, options, tokenService)
    {
        Logger.LogInformation("SeasonRepository doing its thing.");
    }


    // TODO: Should this be defined in the interface or inherated.
    // TODO: Async correction in other methods....  they are not async.
    public async override Task<IEnumerable<Season>> GetAll(CancellationToken token = default)
    {
        // TODO: Apparently this returns all the stuff, i.e. xxx farms.  What we need to do is create the Farm object....
        var myList = getData("seasons");
        var l = new List<Season>();

        // As a test only.... to probe
        string testJson = JsonHelper.Serialize<IList<object>>(myList);
        // File.WriteAllText(string.Format("../../../{0}.json", "testJson"), testJson);

        foreach (object o in myList)
        {
            string jsonO = JsonHelper.Serialize<object>(o);
            var season = JsonHelper.Deserialize<Season>(jsonO);
            l.Add(season);
        }

        string json = JsonHelper.Serialize<IList<Season>>(l);
        // File.WriteAllText(string.Format("../../../{0}.json", "seasons"), json);
        return await Task.FromResult<IEnumerable<Season>>(l);
    }
}