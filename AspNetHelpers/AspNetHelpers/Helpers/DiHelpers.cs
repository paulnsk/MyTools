using Microsoft.Extensions.Options;

namespace AspNetHelpers.Helpers
{
    public static class DiHelpers
    {
        public static T GetJustRegisteredService<T>(this IServiceCollection services) where T : class
        {
            return services.BuildServiceProvider().GetRequiredService<T>();
        }


        public static T AddConfigOptions<T>(this IServiceCollection services, Action<T>? configAction = default) where T : class
        {

            var optionsBuilder = services.AddOptions<T>()
                .BindConfiguration(nameof(T));
            if (configAction != null) optionsBuilder.Configure(configAction);
            optionsBuilder.ValidateDataAnnotations();
            return services.GetJustRegisteredService<IOptions<T>>().Value;
        }
    }
}
