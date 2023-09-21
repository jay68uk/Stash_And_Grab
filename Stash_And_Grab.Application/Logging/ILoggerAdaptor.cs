namespace Stash_And_Grab.Application.Logging;

public interface ILoggerAdaptor<TType>
{
    void LogInformation(string? message, params object?[] args);

    void LogError(Exception exception, string message, params object?[] args);
}