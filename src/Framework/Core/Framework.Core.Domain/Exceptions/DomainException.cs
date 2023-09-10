using System.Runtime.Serialization;
using Framework.Core.Exceptions;

namespace Framework.Core.Domain.Exceptions;

public class DomainException : BaseException, ISerializable
{
    public DomainException() : this(string.Empty, 422)
    {
    }

    public DomainException(string message) : this(message, 422)
    {
    }

    public DomainException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public override bool IsMessageConfidential => false;

    public int ErrorCode { get; private set; }
}