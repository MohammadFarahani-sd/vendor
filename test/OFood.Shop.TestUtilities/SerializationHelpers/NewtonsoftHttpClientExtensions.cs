﻿using System.Text;
using Newtonsoft.Json;

namespace OFood.Shop.TestUtilities.SerializationHelpers;

public static class NewtonsoftHttpClientExtensions
{
    public static async Task<T> GetFromJsonAsync<T>(this HttpClient httpClient, string uri, JsonSerializerSettings? settings = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalidParams(httpClient, uri);

        var response = await httpClient.GetAsync(uri, cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(CancellationToken.None);

        return JsonConvert.DeserializeObject<T>(json, settings)!;
    }

    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string uri, T value, JsonSerializerSettings? settings = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalidParams(httpClient, uri);

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var json = JsonConvert.SerializeObject(value, settings);

        var response = await httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
        
        return response;
    }

    public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string uri, T value, JsonSerializerSettings? settings = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalidParams(httpClient, uri);

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var json = JsonConvert.SerializeObject(value, settings);

        var response = await httpClient.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
        
        return response;
    }
    
    private static void ThrowIfInvalidParams(HttpClient httpClient, string uri)
    {
        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        if (string.IsNullOrWhiteSpace(uri))
        {
            throw new ArgumentException("Can't be null or empty", nameof(uri));
        }
    }
}