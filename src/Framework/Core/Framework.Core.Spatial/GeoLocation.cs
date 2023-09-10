namespace Framework.Core.Spatial;

/// <summary>
///     Latitude lines run east-west and are parallel to each other. If you go north, latitude values increase.
///     Finally, latitude values (Y-values) range between -90 and +90 degrees.
///     But longitude lines run north-south.They converge at the poles.And its X-coordinates are between -180 and +180 degrees.
/// </summary>
[Serializable]
public class GeoLocation
{
    public GeoLocation()
    {
    }

    public GeoLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}