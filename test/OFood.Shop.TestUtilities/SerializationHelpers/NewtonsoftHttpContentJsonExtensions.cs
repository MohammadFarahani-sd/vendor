using Newtonsoft.Json;

namespace OFood.Shop.TestUtilities.SerializationHelpers;

public static class NewtonsoftHttpContentJsonExtensions
{
    public static async Task<T> ReadFromJsonAsync<T>(this HttpContent content, JsonSerializerSettings? settings = null, CancellationToken cancellationToken = default)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        var json = await content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<T>(json, settings)!;
    }
}