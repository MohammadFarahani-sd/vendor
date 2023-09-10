namespace Framework.Core.Exceptions;

[Serializable]
public abstract class BaseException : ApplicationException
{
    protected BaseException()
    {
    }

    protected BaseException(string message) : base(message)
    {
    }

    protected BaseException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public virtual bool IsMessageConfidential => true;
}