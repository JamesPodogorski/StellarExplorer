using System;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;

namespace StellarLib;

public static class JsonHelper
{
    public static T DeserializeFromFile<T>(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions() { IncludeFields = true });
    }

    public static T Deserialize<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions() { IncludeFields = true, });
    }

    public static string Serialize<T>(T t)
    {
        JsonSerializerOptions opt = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            IncludeFields = false
        };
        return JsonSerializer.Serialize<T>(t, opt);
    }

    public static string SerializeWithFields<T>(T t)
    {
        JsonSerializerOptions opt = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            IncludeFields = true
        };
        return JsonSerializer.Serialize<T>(t, opt);
    }

}