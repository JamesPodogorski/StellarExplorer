namespace StellarLib;

using System;

// TODO: Figure out which properties are needed, and then remove those that are not
public class StellarServiceOptions
{
    public string resource { get; set; } = string.Empty;
    public string host { get; set; } = string.Empty;
    public string grant_type { get; set; } = string.Empty;
    public string apiVersion { get; set; } = string.Empty;
    public string tenant_id { get; set; } = string.Empty;
    public string httpClientName { get; set; } = string.Empty;
}