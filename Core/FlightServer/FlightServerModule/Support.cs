namespace OpenVMSys.Core.FlightServerModule;

public static class Support
{
    public static double Dist(double? lat1, double? lon1, double? lat2, double? lon2)
    {
        if (lat1==null || lat2==null || lon1==null || lon2==null)
        {
            return 0;
        }
        var dLon = lon2 - lon1;
        lat1 *= Math.PI / 180;
        lat2 *= Math.PI / 180;
        dLon *= Math.PI / 180;
        var dist = Math.Sin(lat1.Value) * Math.Sin(lat2.Value) + Math.Cos(lat1.Value) * Math.Cos(lat2.Value) * Math.Cos(dLon.Value);
        if (dist > 1.0)
        {
            dist = 1.0;
        }
        dist = Math.Acos(dist) * 60 * 180 / Math.PI;
        return dist;
    }

    public static Server? GetServer(string ident, IEnumerable<Server> list)
    {
        return list.FirstOrDefault(server => server.Ident == ident);
    }

    public static void ClearServerChecks(IEnumerable<Server> list)
    {
        foreach (var server in list)
        {
            server.Check = 0;
        }
    }
}