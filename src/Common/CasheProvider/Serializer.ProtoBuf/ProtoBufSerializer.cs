using System;
using System.IO;
using ProtoBuf.Meta;

namespace Serializer.ProtoBuf
{
    public class ProtoBufSerializer : ISerializer
    {
        public ProtoBufSerializer()
            : this(RuntimeTypeModel.Default)
        {
        }

        public ProtoBufSerializer(RuntimeTypeModel typeModel)
        {
            TypeModel = typeModel;

            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));
        }

        public global::ProtoBuf.Meta.RuntimeTypeModel TypeModel { get; }


        public byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                TypeModel.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public object Deserialize(Type type, byte[] serializedValue)
        {
            using (var stream = new MemoryStream(serializedValue))
            {
                return TypeModel.Deserialize(stream, null, type);
            }
        }

        public T Deserialize<T>(byte[] serializedValue)
        {
            using (var stream = new MemoryStream(serializedValue))
            {
                return (T)TypeModel.Deserialize(stream, null, typeof(T));
            }
        }
    }
}