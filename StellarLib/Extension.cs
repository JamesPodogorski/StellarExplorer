using System;
using System.Text;

namespace StellarLib;

public static class Extension
{
    public static void Path(this UriBuilder builder,
                                 Dictionary<string, string> path)

    {
        path ??= new Dictionary<string, string>();
        StringBuilder sb = new StringBuilder();
        foreach (var val in path)
        {
            sb.AppendFormat(@"/{0}/{1}", val.Key, val.Value);
        }
        builder.Path = sb.ToString();
    }

    public static void Path(this UriBuilder builder, string[] pathParams)

    {
        StringBuilder sb = new StringBuilder();
        foreach (var val in pathParams)
        {
            sb.AppendFormat(@"/{0}", val);
        }
        builder.Path = sb.ToString();
    }

    public static void Query(this UriBuilder builder,
                     Dictionary<string, string> query)
    {
        query ??= new Dictionary<string, string>();
        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (var val in query)
        {
            if (i == 0)
            {
                sb.AppendFormat(@"?{0}={1}", val.Key, val.Value);
                i++;
            }
            else
                sb.AppendFormat(@"&{0}={1}", val.Key, val.Value);

        }
        builder.Query = sb.ToString();
    }

    public static void Query(this UriBuilder builder, (string, string)[] queryParams)
    {
        int i = 0;
        StringBuilder sb = new StringBuilder();
        foreach (var v in queryParams)
        {
            if (i == 0)
            {
                sb.AppendFormat(@"?{0}={1}", v.Item1, v.Item2);
                i++;
            }
            else
                sb.AppendFormat(@"&{0}={1}", v.Item1, v.Item2);
        }
        builder.Query = sb.ToString();
    }
}
