using System;
using System.Linq;
using System.Reflection;
using ProtoBuf.Meta;

namespace Serializer.ProtoBuf
{
    public static class ConfigureTypeProtobufSerializerHelper
    {
        public static bool CanSerialize(this Type type)
        {
            return RuntimeTypeModel.Default.CanSerialize(type);
        }

        public static void ConfigureProtoBufSerialization(this Type type)
        {
            var allProperties = type.GetProperties(BindingFlags.Public
                                                   | BindingFlags.Instance
                                                   | BindingFlags.GetProperty
                                                   | BindingFlags.SetProperty);

            var names = allProperties.Select(x => x.Name).ToArray();

            RuntimeTypeModel.Default.Add(type, false)
                .Add(names);
        }

        public static void ConfigureProtoBufSerialization(this Assembly assembly)
        {
            var candidateTypes = assembly.GetTypes()
                .Where(x =>
                    x.IsPublic &&
                    (x.IsClass) &&
                    x.IsSerializable
                ).ToList();

            foreach (var type in candidateTypes)
            {
                ConfigureProtoBufSerialization(type);
            }
        }
    }
}