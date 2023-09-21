namespace Stash_And_Grab.Application.Logging;

public class LoggerAdaptor<TType> : ILoggerAdaptor<TType>
{
    private readonly ILogger<TType> _logger;

    public LoggerAdaptor(ILogger<TType> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string? message, params object?[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogError(Exception exception, string message, params object?[] args)
    {
        _logger.LogError(exception, message, args);
    }
}