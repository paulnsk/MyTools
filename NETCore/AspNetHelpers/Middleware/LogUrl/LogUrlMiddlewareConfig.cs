namespace YoloApi.Middleware.LogUrl;

public class LogUrlMiddlewareConfig
{
    public bool LogRequestHeaders { get; set; } = false;
    public bool LogExceptionMessage { get; set; } = true;
    public bool LogRequestBody { get; set; } = true;
}