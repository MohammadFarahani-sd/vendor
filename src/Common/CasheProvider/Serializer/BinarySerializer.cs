using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serializer
{
    public class BinarySerializer : ISerializer
    {

        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] serializedValue)
        {
            if (serializedValue == null || serializedValue.Length == 0)
                return default(T);

            using (var ms = new MemoryStream(serializedValue))
            {
                var br = new BinaryFormatter();
                return (T)br.Deserialize(ms);
            }
        }

        public object Deserialize(Type type, byte[] serializedValue)
        {
            if (serializedValue == null || serializedValue.Length == 0)
                return null;

            using (var ms = new MemoryStream(serializedValue))
            {
                var br = new BinaryFormatter();
                return Convert.ChangeType(br.Deserialize(ms), type);
            }
        }
    }
}