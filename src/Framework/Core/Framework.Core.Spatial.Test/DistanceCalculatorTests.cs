namespace Framework.Core.Spatial.Test;

public class DistanceCalculatorTests
{
    [Theory]
    [InlineData(29.29440862831362, 47.901144325733185, 29.2877133, 47.9009867, 744, 745)]
    [InlineData(29.29440862831362, 47.909944325733185, 29.2877133, 47.900486759, 1180, 1182)]
    public void TestingOnDistance(double latitude1, double longitude1, double latitude2, double longitude2, int expectedDistanceLow, int expectedDistanceHigh)
    {
        var actualDistance = DistanceCalculator.Distance(latitude1, longitude1, latitude2, longitude2);
        Assert.InRange(actualDistance, expectedDistanceLow, expectedDistanceHigh);
    }
}