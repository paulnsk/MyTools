using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyTools;

namespace AspNetHelpers.Middleware.LogUrl;

public class LogUrlMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, LogUrlMiddlewareConfig config)
{
    private readonly ILogger<LogUrlMiddleware> _logger = loggerFactory?.CreateLogger<LogUrlMiddleware>() ??
                                                         throw new ArgumentNullException(nameof(loggerFactory));

    public async Task InvokeAsync(HttpContext context)
    {

        var message = $"[{context.Connection.RemoteIpAddress}]: {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}";

        var headers = config.LogRequestHeaders ? string.Join("\n", context.Request.Headers.Select(x => "  ♦m" + x.Key + "♦r: ♦g" + string.Join("; ", x.Value!))) : "";
        if (!string.IsNullOrWhiteSpace(headers)) headers = "\n♦yRequest headers:\n" + headers;

        var error = "";

        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            //unless exception was handled by an exception filter (which it is for validation errors, google SuppressModelStateInvalidFilter to disable)
            error = $" ♦y(♦r{e.ToStringWithInners()}♦y)";
        }

        var statusCode = context.Response.StatusCode;
        var c = "♦g";
        if (statusCode >= 300) c = "♦m";
        if (statusCode >= 400) c = "♦r";
        _logger.LogInformation($"{message}, {c}{statusCode}{error}{headers}");
    }
}