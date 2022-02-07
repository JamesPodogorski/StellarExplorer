using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StellarLib;

public class CropRepository : Repository<Crop>, ICropRepository
{
    private ILogger<CropRepository> logger;
    public CropRepository(ILoggerFactory loggerFactory,
                            IHttpClientFactory httpClientFactory, IOptions<StellarServiceOptions> options,
                            ITokenService tokenService)
                        : base(loggerFactory, httpClientFactory, options, tokenService)
    {
        Logger.LogInformation("CropRepository doing its thing.");
    }

    // TODO: Should this be defined in the interface or inherated.
    // TODO: Async correction in other methods....  they are not async.
    public async override Task<IEnumerable<Crop>> GetAll(CancellationToken token = default)
    {
        // TODO: Apparently this returns all the stuff, i.e. xxx farms.  What we need to do is create the Farm object....
        var myList = getData("crops");
        var l = new List<Crop>();

        // As a test only.... to probe
        string testJson = JsonHelper.Serialize<IList<object>>(myList);
        // File.WriteAllText(string.Format("../../../{0}.json", "testJson"), testJson);

        foreach (object o in myList)
        {
            string jsonO = JsonHelper.Serialize<object>(o);
            var crop = JsonHelper.Deserialize<Crop>(jsonO);
            l.Add(crop);
        }

        string json = JsonHelper.Serialize<IList<Crop>>(l);
        // File.WriteAllText(string.Format("../../../{0}.json", "crops"), json);
        return await Task.FromResult<IEnumerable<Crop>>(l);
    }
}