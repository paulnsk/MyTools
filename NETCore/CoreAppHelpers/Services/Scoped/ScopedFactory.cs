using Microsoft.Extensions.DependencyInjection;

namespace CoreAppHelpers.Services.Scoped;

public class ScopedFactory<T>(IServiceScopeFactory scopeFactory) : IScopedFactory<T>
{
    public Scoped<T> Create()
    {
        var scope = scopeFactory.CreateScope();
        var t = scope.ServiceProvider.GetRequiredService<T>();
        return new Scoped<T>(t, scope);
    }
}