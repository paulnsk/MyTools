namespace AspNetHelpers.Middleware.LogUrl;

public class LogUrlMiddlewareConfig
{
    public bool LogRequestHeaders { get; set; } = false;
    public bool LogExceptionMessage { get; set; } = true;
}