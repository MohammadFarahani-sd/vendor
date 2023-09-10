using Flurl;

namespace OFood.Shop.TestUtilities.UriHelpers;

public class UriHelper
{
    public static string BuildRelativeUrl(HttpClient client, string schemeSegment, object obj)
    {
        var uri = client.BaseAddress?.ToString()
            .AppendPathSegment(schemeSegment)
            .SetQueryParams(obj)
            .ToUri();
        var relativeUri = $"{schemeSegment}{uri?.MakeRelativeUri(uri)}";
        return relativeUri;
    }
}