using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreAppHelpers.Helpers
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
                .BindConfiguration(typeof(T).Name);
            //todo_ акшен вызывается больше одного раза. Надо б подебажить, хотя вроде вреда не причиняет
            if (configAction != null) optionsBuilder.Configure(configAction);
            optionsBuilder.ValidateDataAnnotations();
            return services.GetJustRegisteredService<IOptions<T>>().Value;
        }
    }
}
