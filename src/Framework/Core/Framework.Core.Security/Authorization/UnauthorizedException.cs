using Framework.Core.Exceptions;

namespace Framework.Core.Security.Authorization;

[Serializable]
public class UnauthorizedException : SecurityException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innException) : base(message, innException)
    {
    }
}