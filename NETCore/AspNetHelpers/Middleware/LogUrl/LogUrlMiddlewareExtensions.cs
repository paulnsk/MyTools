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

        public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogUrlMiddleware>();
        }
    }
}
