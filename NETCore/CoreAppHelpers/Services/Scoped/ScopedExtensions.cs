using Microsoft.Extensions.DependencyInjection;

namespace CoreAppHelpers.Services.Scoped
{
    public static class ScopedExtensions
    {
        public static IServiceCollection AddScopedFactory(this IServiceCollection services)
        {
            services.AddScoped(typeof(Scoped<>));
            services.AddScoped(typeof(IScopedFactory<>), typeof(ScopedFactory<>));
            return services;
        }
    }
}
