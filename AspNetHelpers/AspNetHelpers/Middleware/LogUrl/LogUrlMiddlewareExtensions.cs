using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace AspNetHelpers.Middleware.LogUrl
{
    public static class LogUrlMiddlewareExtensions
    {

        public static IServiceCollection AddLogUrlMiddleware(this IServiceCollection services)
        {
            services.AddOptions<LogUrlMiddlewareConfig>()
                .BindConfiguration(nameof(LogUrlMiddlewareConfig))
                .ValidateDataAnnotations();
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
