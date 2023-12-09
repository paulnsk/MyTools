using Microsoft.Extensions.Options;
using CoreAppHelpers.Helpers;

namespace AspNetHelpers.Middleware.LogUrl
{
    public static class LogUrlMiddlewareExtensions
    {

        public static IServiceCollection AddLogUrlMiddleware(this IServiceCollection services, Action<LogUrlMiddlewareConfig>? configAction = default)
        {
            services.AddConfigOptions(configAction);
            return services;
        }

        public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app, Action<LogUrlMiddlewareConfig>? configAction = default)
        {
            var config = app.ApplicationServices.GetService<IOptions<LogUrlMiddlewareConfig>>()?.Value ?? new LogUrlMiddlewareConfig();

            configAction?.Invoke(config);

            return app.UseMiddleware<LogUrlMiddleware>(config);
        }
    }
}
