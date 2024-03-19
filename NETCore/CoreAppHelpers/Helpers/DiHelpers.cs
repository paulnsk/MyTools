using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreAppHelpers.Helpers
{
    public static class DiHelpers
    {
        //Use with care as this may lead to services being unexpectedly duplicated
        private static T GetJustRegisteredService<T>(this IServiceCollection services) where T : class
        {
            using var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<T>();
        }

        public static void AddConfigOptions<T>(this IServiceCollection services, Action<T>? configAction = default) where T : class
        {
            var optionsBuilder = services.AddOptions<T>().BindConfiguration(typeof(T).Name).ValidateDataAnnotations();
            //todo_ акшен вызывается больше одного раза. Надо б подебажить, хотя вроде вреда не причиняет
            if (configAction != null) services.Configure(configAction);
        }

        public static TConfig GetJustRegisteredConfig<TConfig>(this IServiceCollection services) where TConfig : class
        {
            return services.GetJustRegisteredService<IOptions<TConfig>>().Value;
        }
    }
}
