using Newtonsoft.Json;

namespace MapService;

public class ParsiMapApiResponse
{
    [JsonProperty("address")]
    public string Address { get; set; } = null!;

    [JsonProperty("subdivision_prefix")]
    public string SubdivisionPrefix { get; set; } = null!;

    [JsonProperty("subdiv_prefixed_address")]
    public string SubdivPrefixedAddress { get; set; } = null!;
}

