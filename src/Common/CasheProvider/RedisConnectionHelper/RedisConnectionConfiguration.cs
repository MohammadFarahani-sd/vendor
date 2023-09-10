namespace RedisConnectionHelper
{
    public class RedisConnectionConfiguration
    {
        public string Connection { get; set; }
        public int ConnectionCount { get; set; }
        public string Password { get; set; }
        public bool UseSSl { get; set; }
    }
}