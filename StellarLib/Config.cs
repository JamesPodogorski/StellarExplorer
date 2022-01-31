namespace StellarLib;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using System.Net.Http;

public static class Config<T>
{
    private static T _config;
    public static T LoadConfig(string configFile)
    {
        if (_config == null)
        {
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(configFile)
            .Build();
            var section = configBuilder.GetSection(typeof(T).Name);
            _config = section.Get<T>();

        }
        return _config;
    }
}