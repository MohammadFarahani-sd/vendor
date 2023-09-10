namespace OFood.Shop.Api.Configurations.EventBus;

public sealed class EventBusSetting
{
    public string HostName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RetryCount { get; set; } = null!;
    public int Port { get; set; }
    public string VirtualHost { get; set; } = null!;
    public string BrokerPrefixName { get; set; } = null!;
}