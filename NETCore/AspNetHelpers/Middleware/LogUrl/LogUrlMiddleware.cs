using ConsoleTools;
using Microsoft.Extensions.Options;
using YoloApi.Middleware.LogUrl;

namespace AspNetHelpers.Middleware.LogUrl;


public static class ExtMeth
{
    // copied from Utils
    public static string Ellipsize(this string @this, int? maxLength = 100)
    {
        //ellipsizing turned off
        if (maxLength == null) return @this;

        const string ellipsis1 = "...< ";
        const string ellipsis2 = " >...";
        const int ending = 5;
        const string text = " more chars";
        var length = @this.Length;
        var lengthLength = length.ToString().Length;

        var extraChars = ellipsis1.Length + ellipsis2.Length + text.Length + lengthLength;

        if (maxLength < extraChars + ending + 1) return @this;
        if (length <= maxLength) return @this;

        var before = @this[..(maxLength.Value - ending - extraChars)];
        var after = @this[(length - ending)..];

        return $"{before}{ellipsis1}{length - ending - extraChars}{text}{ellipsis2}{after}";
    }
}

public class LogUrlMiddleware(RequestDelegate next, ILogger<LogUrlMiddleware> logger, IOptions<LogUrlMiddlewareConfig> configOptions)
{
    private readonly LogUrlMiddlewareConfig _config = configOptions.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = new MyIpAddress(context.Connection.RemoteIpAddress);
        var requestUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);
        var baseMessage = $"[♣G{ip}♣=]: <♣M{context.Request.Method}♣=> ♣b{requestUrl}♣=";

        
        var headers = _config.LogRequestHeaders
            ? string.Join("\n", context.Request.Headers.Select(x => "  ♦m" + x.Key + "♦r: ♦g" + string.Join("; ", x.Value.ToArray())))
            : "";
        if (!string.IsNullOrWhiteSpace(headers)) headers = "\n♦yRequest headers:\n" + headers;

        
        var requestBody = "";
        if (_config.LogRequestBody && context.Request.Method == "POST")
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(
                context.Request.Body,
                encoding: System.Text.Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            if (!string.IsNullOrWhiteSpace(requestBody))
                requestBody = "\n♦yRequest body:\n♦=" + requestBody.ToSyntaxHighlightedJson().Ellipsize(500);
        }

        
        logger.LogInformation($"{baseMessage}♣= [♣gbegin♣=]{headers}{requestBody}");

        
        await next(context);

        
        var statusCode = context.Response.StatusCode;
        var c = "♦g";
        if (statusCode >= 300) c = "♦m";
        if (statusCode >= 400) c = "♦r";
        
        logger.LogInformation($"{baseMessage}, {c}{statusCode}♣= [{c}completed♣=]");
    }
}