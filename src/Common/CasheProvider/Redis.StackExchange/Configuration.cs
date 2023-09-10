using Serializer;

namespace Redis.StackExchange
{
    public class Configuration
    {
        protected Configuration(ISerializer serializer)
        {
            Serializer = serializer;
        }

        public static Configuration Default { get; }
            = new Configuration(new BinarySerializer());


        public ISerializer Serializer { get; }
    }
}