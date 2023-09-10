namespace Framework.Core.ExceptionHandling;

public interface IExceptionHandler
{
    void Handle(object context, Exception ex);
}