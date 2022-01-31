// using System;
// using Microsoft.Extensions.Logging;

// namespace StellarLib;

// public class OkRepository : Repository<Ok>, IOkRepository
// {
//     public OkRepository(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
//                         : base(loggerFactory, httpClientFactory, null, null)
//     {
//         var logger = loggerFactory.CreateLogger<OkRepository>();
//         logger.LogInformation("Ok Repository");
//     }

//     public async override Task<Ok> GetById(string id)
//     {
//         HttpClient client = CreateHttpClient();
//         return await Task.FromResult<Ok>(new Ok());
//     }
// }