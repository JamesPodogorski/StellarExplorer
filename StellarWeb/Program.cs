using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StellarWeb.Data;
using StellarLib;
using StellarWeb.Shared;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Configuration;

// string configFile = string.Empty;
// TODO: How do we make this configuration setting file test/dev vs production
string configFile = "appsettings.json";
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
// builder.Services.AddLogging();

// configureStellarServices();
// configureTokenService();
configureFarmerService();
// configureCORS();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// app.UseCors();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

void configureFarmerService()
{
    var tokenServiceOptions = WebHelper.GetServiceOptions<TokenServiceOptions>(WebHelper.TokenOptionSectionName);
    var stellarServiceOptions = WebHelper.GetServiceOptions<StellarServiceOptions>(WebHelper.StellarConfigSection);

    // //* Service Collection
    // var serviceCollection = new ServiceCollection().AddLogging(options =>
    // {
    //     options.AddConsole();
    // });

    //* Services
    builder.Services.AddTransient<IFarmerRepository, FarmerRepository>();
    builder.Services.AddTransient<IFarmRepository, FarmRepository>();
    builder.Services.AddTransient<IFieldRepository, FieldRepository>();
    builder.Services.AddTransient<IBoundaryRepository, BoundaryRepository>();
    builder.Services.AddTransient<ICropRepository, CropRepository>();
    builder.Services.AddTransient<ISeasonRepository, SeasonRepository>();
    builder.Services.AddTransient<ITokenService, TokenService>();

    //* Htpp Clients
    // TODO: add retry policies to http clients
    // TODO: fix the BaseAddress for our Stellar Service
    builder.Services.AddHttpClient(stellarServiceOptions.httpClientName, client =>
    {
        // client.BaseAddress = new Uri("https://www.asdf.com");
    });

    builder.Services.AddHttpClient(tokenServiceOptions.httpClientName, client =>
    {
    });

    //* Service Config
    builder.Services.Configure<TokenServiceOptions>(options =>
    {
        options.applicationSecret = tokenServiceOptions.applicationSecret;
        options.clientId = tokenServiceOptions.clientId;
        options.httpClientName = tokenServiceOptions.httpClientName;
        options.resource = tokenServiceOptions.resource;
        options.tenantId = tokenServiceOptions.tenantId;
    });

    builder.Services.Configure<StellarServiceOptions>(options =>
    {
        options.apiVersion = stellarServiceOptions.apiVersion;
        options.grant_type = stellarServiceOptions.grant_type;
        options.host = stellarServiceOptions.host;
        options.resource = stellarServiceOptions.resource;
        options.tenant_id = stellarServiceOptions.tenant_id;
        options.httpClientName = stellarServiceOptions.host;
    });

    // //* Get instance and test service call
    // var provider = serviceCollection.BuildServiceProvider();
}

void configureCORS()
{
    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder.WithOrigins(
                                    "https://farmbeats.azure.net",
                                    "https://login.microsoftonline.com"
                                    ).AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
            });
    });
}

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
    builder.Services.AddHttpClient("token", client =>
    {
        client.BaseAddress = new Uri(string.Format(@"{0}://login.microsoftonline.com/{1}/oauth2/token", scheme, config.clientId));
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

