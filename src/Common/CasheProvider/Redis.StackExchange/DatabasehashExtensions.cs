using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Serializer;
using StackExchange.Redis;

namespace Redis.StackExchange
{
    public static class DatabaseHashExtensions
    {
        /// <summary>
        /// Delegate used to set redis values to instance properties and fields
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        private delegate void SetValueDelegate(object obj, RedisValue value, ISerializer serializer);

        /// <summary>
        /// Delegate used to cast common clr types to redis values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private delegate RedisValue CastToRedisValueDelegate(object obj);

        private class TypeDescriptors
        {
            public Tuple<PropertyInfo, CastToRedisValueDelegate>[] Props { get; set; }

            public Tuple<FieldInfo, CastToRedisValueDelegate>[] Fields { get; set; }

            public Dictionary<string, SetValueDelegate> Setters { get; } =
                new Dictionary<string, SetValueDelegate>();
        }

        /// <summary>
        /// Used to store mapping delegates for types
        /// </summary>
        private static readonly ConcurrentDictionary<Type, TypeDescriptors>
            Descriptors = new ConcurrentDictionary<Type, TypeDescriptors>();

        /// <summary>
        /// These types are supported by RedisValue to be converted implicitly
        /// </summary>
        private static readonly HashSet<Type> NoSerializeSupportedTypes = new HashSet<Type>(
            new[]
            {
                typeof(bool),
                typeof(bool?),
                typeof(int),
                typeof(int?),
                typeof(long),
                typeof(long?),
                typeof(double),
                typeof(double?),
                typeof(string),

            }
        );

        /// <summary>
        /// Used to store delegates for converting usual clr types to redis values
        /// </summary>
        private static readonly ConcurrentDictionary<Type, CastToRedisValueDelegate> CastDelegatesCache
            = new ConcurrentDictionary<Type, CastToRedisValueDelegate>();


        public static Task HashSetAsync<T>(this IDatabase database,
            RedisKey key, T value
            , TimeSpan? expiration = null
            , CommandFlags flags = CommandFlags.None
            , string isNullIndicator = null)
            where T : new()
        {
            return database.HashSetAsync<T>(
                key: key,
                value: value,
                serializer: Configuration.Default.Serializer,
                expiration: expiration,
                flags: flags,
                isNullIndicator: isNullIndicator);
        }

        public static Task HashSetAsync<T>(this IDatabase database,
            RedisKey key, T value,
            ISerializer serializer
            , TimeSpan? expiration = null
            , CommandFlags flags = CommandFlags.None
            , string isNullIndicator = null)
        where T : new()
        {
            Task task = database.HashSetAsync(key, ConvertValue(value, serializer, isNullIndicator), flags);

            return expiration.HasValue
                ? task.ContinueWith(t => database.KeyExpireAsync(key, expiration, flags)
                    , TaskContinuationOptions.OnlyOnRanToCompletion)
                : task;
        }

        public static Task<T> HashGetAsync<T>(this IDatabase database,
            RedisKey key,
            CommandFlags flags = CommandFlags.None)
            where T : new()
        {
            return database.HashGetAsync<T>(
                key: key,
                serializer: Configuration.Default.Serializer,
                flags: flags);
        }

        public static async Task<T> HashGetAsync<T>(this IDatabase database,
            RedisKey key,
            ISerializer serializer,
            CommandFlags flags = CommandFlags.None)
        where T : new()
        {
            HashEntry[] entires = await database.HashGetAllAsync(key, flags);

            if (entires == null || entires.Length == 0)
                return default;

            return ConvertEntries<T>(entires, serializer);

        }

        private static HashEntry[] ConvertValue(object obj, ISerializer serializer, string isNullIndicator)
        {
            Type type = obj.GetType();
            if (!Descriptors.TryGetValue(type, out TypeDescriptors descriptors))
                descriptors = CreateTypeDescriptors(type);

            var entries = new List<HashEntry>(descriptors.Props.Length + descriptors.Fields.Length);

            foreach (Tuple<PropertyInfo, CastToRedisValueDelegate> t in descriptors.Props)
            {
                PropertyInfo pinfo = t.Item1;
                CastToRedisValueDelegate castDelegate = t.Item2;
                object propertyValue = pinfo.GetValue(obj);

                if (propertyValue == null)
                    continue;

                RedisValue entryValue;
                if (NoSerializeSupportedTypes.Contains(pinfo.PropertyType))
                    entryValue = castDelegate(propertyValue);
                else
                    entryValue = ConvertToBytes(propertyValue, serializer);

                entries.Add(new HashEntry(pinfo.Name, entryValue));
            }

            foreach (Tuple<FieldInfo, CastToRedisValueDelegate> t in descriptors.Fields)
            {
                FieldInfo finfo = t.Item1;
                CastToRedisValueDelegate castDelegate = t.Item2;
                object fieldValue = finfo.GetValue(obj);
                if (fieldValue == null)
                    continue;


                RedisValue entryValue;
                if (NoSerializeSupportedTypes.Contains(finfo.FieldType))
                    entryValue = castDelegate(fieldValue);
                else
                    entryValue = ConvertToBytes(fieldValue, serializer);

                entries.Add(new HashEntry(finfo.Name, entryValue));
            }


            if (entries.Count == 0)
                if (string.IsNullOrWhiteSpace(isNullIndicator))
                    throw new ArgumentException("Unable to serialize object which all the object properties and fields are null");
                else
                    entries.Add(new HashEntry(isNullIndicator, true));


            return entries.ToArray();
        }

        private static T ConvertEntries<T>(HashEntry[] entries, ISerializer serializer)
            where T : new()
        {
            var obj = new T();
            Type type = typeof(T);


            if (!Descriptors.TryGetValue(type, out TypeDescriptors descriptors))
                descriptors = CreateTypeDescriptors(type);

            foreach (HashEntry entry in entries)
            {
                if (!descriptors.Setters.TryGetValue(entry.Name, out SetValueDelegate setter))
                    continue;

                setter(obj, entry.Value, serializer);
            }

            return obj;
        }

        private static RedisValue ConvertToBytes(object obj, ISerializer serializer)
        {
            if (obj == null)
                return RedisValue.Null;

            return serializer.Serialize(obj);
        }

        private static void SetNonSerializedPropertyValue<TValue>(PropertyInfo pinfo,
            object obj, RedisValue redisValue, ISerializer serializer)
        {
            if (redisValue.IsNull)
                return;

            object value = Convert.ChangeType(redisValue, typeof(TValue));
            pinfo.SetValue(obj, value);
        }

        private static void SetSerializedPropertyValue<TValue>(PropertyInfo pinfo,
            object obj, RedisValue redisValue, ISerializer serializer)
        {
            if (redisValue.IsNull)
                return;

            object value = serializer.Deserialize<TValue>(redisValue);
            pinfo.SetValue(obj, value);
        }

        private static void SetNonSerializedFieldValue<TValue>(FieldInfo finfo,
            object obj, RedisValue redisValue, ISerializer serializer)
        {
            if (redisValue.IsNull)
                return;

            object value = Convert.ChangeType(redisValue, typeof(TValue));
            finfo.SetValue(obj, value);
        }

        private static void SetSerializedFieldValue<TValue>(FieldInfo finfo,
            object obj, RedisValue redisValue, ISerializer serializer)
        {
            if (!redisValue.HasValue)
                return;

            object value = serializer.Deserialize<TValue>(redisValue);
            finfo.SetValue(obj, value);
        }

        private static TypeDescriptors CreateTypeDescriptors(Type type)
        {
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public
                                           | BindingFlags.Instance
                                           | BindingFlags.GetProperty
                                           | BindingFlags.SetProperty);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public
                                        | BindingFlags.Instance
                                        | BindingFlags.GetField
                                        | BindingFlags.SetField);

            var descriptors = new TypeDescriptors();

            descriptors.Props = props
                .Where(pinfo => pinfo.CanRead && pinfo.CanWrite)
                .Select(pinfo =>
                {
                    Type pType = pinfo.PropertyType;
                    bool noSerialization = NoSerializeSupportedTypes.Contains(pType);

                    MethodInfo setterMethodInfoGeneric = typeof(DatabaseHashExtensions)
                        .GetMethod(noSerialization
                                ? nameof(SetNonSerializedPropertyValue)
                                : nameof(SetSerializedPropertyValue)
                            , BindingFlags.NonPublic | BindingFlags.Static);

                    if (setterMethodInfoGeneric == null)
                        throw new Exception("Impossible");


                    Type setterValueType = Nullable.GetUnderlyingType(pType)
                                ?? pType;
                    MethodInfo setterMethodInfo = setterMethodInfoGeneric
                        .MakeGenericMethod(setterValueType);


                    var setter = (SetValueDelegate)Delegate.CreateDelegate(typeof(SetValueDelegate)
                        , pinfo, setterMethodInfo);

                    // add to reverse dictionary
                    descriptors.Setters.Add(pinfo.Name, setter);

                    // Create cast from property type to redisValue delegate
                    CastToRedisValueDelegate castDelegate =
                        noSerialization
                            ? CreateCastDelegate(pType)
                            : null;

                    return Tuple.Create(pinfo, castDelegate);
                }).ToArray();

            descriptors.Fields = fields
                .Select(finfo =>
                {
                    Type fType = finfo.FieldType;
                    bool noSerialization = NoSerializeSupportedTypes.Contains(fType);

                    MethodInfo setterMethodInfoGeneric = typeof(DatabaseHashExtensions)
                        .GetMethod(noSerialization
                                ? nameof(SetNonSerializedFieldValue)
                                : nameof(SetSerializedFieldValue)
                            , BindingFlags.NonPublic | BindingFlags.Static);

                    if (setterMethodInfoGeneric == null)
                        throw new Exception("Impossible");

                    Type setterValueType = Nullable.GetUnderlyingType(fType)
                                           ?? fType;
                    MethodInfo setterMethodInfo = setterMethodInfoGeneric
                        .MakeGenericMethod(setterValueType);

                    var setter = (SetValueDelegate)Delegate.CreateDelegate(typeof(SetValueDelegate)
                        , finfo, setterMethodInfo);

                    // add to reverse dictionary
                    descriptors.Setters.Add(finfo.Name, setter);

                    // Create cast from field type to redisValue delegate
                    CastToRedisValueDelegate castDelegate =
                        noSerialization
                            ? CreateCastDelegate(fType)
                            : null;

                    return Tuple.Create(finfo, castDelegate);
                }).ToArray();

            Descriptors.TryAdd(type, descriptors);

            return descriptors;
        }

        private static CastToRedisValueDelegate CreateCastDelegate(Type type)
        {
            if (CastDelegatesCache.TryGetValue(type, out CastToRedisValueDelegate castDelegate))
                return castDelegate;


            MethodInfo createGenericCastDelegateMethodInfo
                = typeof(DatabaseHashExtensions)
                    .GetMethod(nameof(CreateGenericCastDelegate),
                        BindingFlags.Static | BindingFlags.NonPublic);

            if (createGenericCastDelegateMethodInfo == null)
                throw new Exception("Impossible");

            // this is of type Func<type, RedisValue>
            object createGenericCastDelegate
                = createGenericCastDelegateMethodInfo.MakeGenericMethod(type)
                    .Invoke(null, new object[0]);


            if (createGenericCastDelegate == null)
                throw new Exception("Impossible");

            // create a delegate of type CastDelegate using CastObjectToRedisValue

            // find generic method info
            MethodInfo castObjectToRedisValueGenericMethodInfo
                = typeof(DatabaseHashExtensions)
                    .GetMethod(nameof(CastObjectToRedisValue),
                        BindingFlags.Static | BindingFlags.NonPublic);

            if (castObjectToRedisValueGenericMethodInfo == null)
                throw new Exception("Impossible");

            // Create the concrete method
            MethodInfo castObjectToRedisValueMethodInfo
                = castObjectToRedisValueGenericMethodInfo.MakeGenericMethod(
                    type);
            if (castObjectToRedisValueMethodInfo == null)
                throw new Exception("Impossible");

            castDelegate = (CastToRedisValueDelegate)Delegate.CreateDelegate(typeof(CastToRedisValueDelegate),
                createGenericCastDelegate,
                castObjectToRedisValueMethodInfo);

            CastDelegatesCache.TryAdd(type, castDelegate);

            return castDelegate;
        }

        private static Func<T, RedisValue> CreateGenericCastDelegate<T>()
        {
            MethodInfo implicitOpMethodInfo
                = typeof(RedisValue).GetMethod("op_Implicit", new[]
                {
                    typeof(T)
                });

            if (implicitOpMethodInfo == null)
                throw new Exception("Impossible");

            return (Func<T, RedisValue>)implicitOpMethodInfo
                .CreateDelegate(typeof(Func<T, RedisValue>));
        }

        private static RedisValue CastObjectToRedisValue<T>(Func<T, RedisValue> func,
            object obj)
        {
            return func((T)obj);
        }

    }
}