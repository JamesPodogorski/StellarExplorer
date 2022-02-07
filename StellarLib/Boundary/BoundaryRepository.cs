using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StellarLib;

public class BoundaryRepository : Repository<Boundary>, IBoundaryRepository
{
    private ILogger<BoundaryRepository> logger;
    public BoundaryRepository(ILoggerFactory loggerFactory,
                            IHttpClientFactory httpClientFactory, IOptions<StellarServiceOptions> options,
                            ITokenService tokenService)
                        : base(loggerFactory, httpClientFactory, options, tokenService)
    {
        Logger.LogInformation("Boundary Repository doing its thing.");
    }

    // TODO: Should this be defined in the interface or inherated.
    // TODO: Async correction in other methods....  they are not async.
    public async override Task<IEnumerable<Boundary>> GetAll(CancellationToken token = default)
    {
        // TODO: Apparently this returns all the stuff, i.e. xxx farms.  What we need to do is create the Farm object....
        var myList = getData("boundaries");
        var l = new List<Boundary>();

        // As a test only.... to probe
        string testJson = JsonHelper.Serialize<IList<object>>(myList);
        // File.WriteAllText(string.Format("../../../{0}.json", "testJson"), testJson);

        // int i = 1;
        foreach (object o in myList)
        {
            // For Boundaries, the serialized json includes an Id but our deserialized boundary does not, fix the return values...
            string jsonO = JsonHelper.Serialize<object>(o);
            var boundary = JsonHelper.Deserialize<Boundary>(jsonO);
            // TODO: numb is only for counting/verification purposes.  Remove during clean up.
            // boundary.numb = i++;
            l.Add(boundary);
        }
   
        string json = JsonHelper.Serialize<IList<Boundary>>(l);
        // File.WriteAllText(string.Format("../../../{0}.json", "boundaries"), json);
        return await Task.FromResult<IEnumerable<Boundary>>(l);
    }
}
