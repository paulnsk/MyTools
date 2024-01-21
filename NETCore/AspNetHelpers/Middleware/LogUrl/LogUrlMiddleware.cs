using AspNetHelpers.ErrorFiltering;
using AspNetHelpers.Utils;
using Microsoft.Extensions.Options;
using System.IO;

namespace AspNetHelpers.Middleware.LogUrl;


/// <summary>
/// This middleware is intended to use in conjunction with KonsoleLogger for message coloring.
/// - Logs requests with returned status codes
/// - Catches unhandled exceptions and logs them (client gets a "some error occurred" generic message)
/// - For handled (by other middleware/filters) exceptions: logs response body
/// </summary>
public class LogUrlMiddleware(RequestDelegate next, ILogger<LogUrlMiddleware> logger, IOptions<LogUrlMiddlewareConfig> configOptions)
{
    private readonly LogUrlMiddlewareConfig _config = configOptions.Value;

    //todo log request method and optionally request body

    public async Task InvokeAsync(HttpContext context)
    {
        _context = context;

        var ip = new MyIpAddress(context.Connection.RemoteIpAddress);

        var message = $"[{ip}]: {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}";

        var headers = _config.LogRequestHeaders ? string.Join("\n", context.Request.Headers.Select(x => "  ♦m" + x.Key + "♦r: ♦g" + string.Join("; ", x.Value!))) : "";
        if (!string.IsNullOrWhiteSpace(headers)) headers = "\n♦yRequest headers:\n" + headers;

        var error = "";

        PrepareBodyStream();

        var exceptionWasHandledByOtherMiddleware = true;

        try
        {
            // - generic exceptions are logged but not shown to client
            // - filtered errors such as StatusCodeException or validation errors are not thrown here.
            //   Assuming that that the filter does put them in the response body
            //   we log such bodies below (on a consition that status code is not OK and exception was not thrown)
            //   (to disable validation error filter google SuppressModelStateInvalidFilter (bad idea))
            await next(context);
        }
        catch (Exception e)
        {
            exceptionWasHandledByOtherMiddleware = false;

            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var someId = $"<{Guid.NewGuid():N}>";

            await context.Response.WriteAsync("An error occurred." + (_config.LogExceptionMessage ? $" See server log for details {someId}." : ""));

            error = _config.LogExceptionMessage ? $" ♦y(♦r{someId}{e.ToStringWithInners()}♦y)" : "";
        }
        
        //restoring streams
        var responseBodyTask = ReadResponseBody();

        var statusCode = context.Response.StatusCode;

        if (statusCode >= 300 && exceptionWasHandledByOtherMiddleware)
        {
            error = _config.LogExceptionMessage ? $" ♦y(♦r{await responseBodyTask}♦y)" : "";
        }

        var c = "♦g";
        if (statusCode >= 300) c = "♦m";
        if (statusCode >= 400) c = "♦r";
        logger.LogInformation($"{message}, {c}{statusCode}{error}{headers}");
    }


    private HttpContext? _context;
    private Stream? _originalBodyStream;
    private MemoryStream? _memoryStream;
    private async Task<string> ReadResponseBody()
    {
        if (!_config.LogExceptionMessage) return string.Empty;
        if (_memoryStream == null || _originalBodyStream == null || _context == null) throw new Exception($"Error in {nameof(LogUrlMiddleware)}: streams or context not assigned!");
        _memoryStream.Position = 0;
        await _memoryStream.CopyToAsync(_originalBodyStream);
        _memoryStream.Position = 0;
        using var reader = new StreamReader(_memoryStream);
        var responseBody = await reader.ReadToEndAsync();
        _context.Response.Body = _originalBodyStream;
        await _memoryStream.DisposeAsync();
        return responseBody;
    }

    
    private void PrepareBodyStream()
    {
        if (_context == null) throw new Exception($"Error in {nameof(LogUrlMiddleware)}: context not assigned!");
        if (!_config.LogExceptionMessage) return;
        _originalBodyStream = _context.Response.Body;
        _memoryStream = new MemoryStream();
        _context.Response.Body = _memoryStream;
    }


}

internal static class ExtMeth
{
    public static string ToStringWithInners(this Exception? @this)
    {
        if (@this == null) return "";
        var result = "[" + @this.GetType() + "]: " + @this.Message;
        if (@this.InnerException != null) result += "; Inner: " + @this.InnerException.ToStringWithInners();
        return result;
    }
}