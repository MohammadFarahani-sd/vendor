using System;
using ProtoBuf;

namespace Serializer.ProtoBuf
{
    [ProtoContract]
    public class DateTimeOffsetSurrogate
    {
        [ProtoMember(1)]
        public long UtcTicks { get; set; }

        public static implicit operator DateTimeOffsetSurrogate(DateTimeOffset value)
        {
            return new DateTimeOffsetSurrogate { UtcTicks = value.UtcTicks };
        }

        public static implicit operator DateTimeOffset(DateTimeOffsetSurrogate value)
        {
            return new DateTimeOffset(value.UtcTicks,TimeSpan.Zero);
        }
    }
}