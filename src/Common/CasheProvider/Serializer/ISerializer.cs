using System;

namespace Serializer
{
    public interface ISerializer
    {
        byte[] Serialize(object obj);

        T Deserialize<T>(byte[] serializedValue);

        object Deserialize(Type type, byte[] serializedValue);
    }
}