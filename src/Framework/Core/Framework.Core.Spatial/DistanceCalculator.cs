namespace Framework.Core.Spatial;

public static class DistanceCalculator
{
    public static double Distance(double lat1, double lon1, double lat2, double lon2)
    {
        lon1 = ToRadians(lon1);
        lon2 = ToRadians(lon2);
        lat1 = ToRadians(lat1);
        lat2 = ToRadians(lat2);

        var dlon = lon2 - lon1;
        var dlat = lat2 - lat1;
        var a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        var c = 2 * Math.Asin(Math.Sqrt(a));
        double r = 6371000;
        return c * r;
    }

    private static double ToRadians(double angleIn10thofaDegree)
    {
        return angleIn10thofaDegree * Math.PI / 180;
    }
}