namespace Framework.Core.Exceptions;

[Serializable]
public abstract class SecurityException : BaseException
{
    protected SecurityException()
    {
    }

    protected SecurityException(string message) : base(message)
    {
    }

    protected SecurityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}