using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StellarWeb.Data;
using StellarLib;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Configuration;

string configFile = string.Empty;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

configureStellarServices();
configureTokenService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

void configureStellarServices()
{
        //     httpClient.DefaultRequestHeaders.Add(
        //     HeaderNames.Accept, "application/vnd.github.v3+json");
        // httpClient.DefaultRequestHeaders.Add(
        //     HeaderNames.UserAgent, "HttpRequestsSample");

    builder.Services.AddTransient<IStellarService, StellarService>();
    builder.Services.AddHttpClient("stellar", client =>
    {
        client.BaseAddress = new Uri("https://www.ddd.com");
    }).AddPolicyHandler(p => 
    {
        return GetPolicy();
    });
}

void configureTokenService()
{
    string scheme = "https";
    var config = getConfig<TokenServiceOptions>();
    builder.Services.Configure<TokenServiceOptions>(option => 
    {
        option = config;
    });

    builder.Services.AddSingleton<TokenService>();
    builder.Services.AddHttpClient("token", client => {
        client.BaseAddress = new Uri(string.Format(@"{0}://login.microsoftonline.com/{1}/oauth2/token", scheme, config.clientId));
    }).AddPolicyHandler(p => 
    {
        return GetPolicy();
    });
}

IAsyncPolicy<HttpResponseMessage> GetPolicy()
{
    return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

T getConfig<T>()
{
    var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(configFile)
                .AddEnvironmentVariables("ooo")
                .Build();
                var section = configBuilder.GetSection(typeof(T).Name);
                var o = configBuilder.GetValue<string>("asdf");
                var config = section.Get<T>();
    return config;
}

