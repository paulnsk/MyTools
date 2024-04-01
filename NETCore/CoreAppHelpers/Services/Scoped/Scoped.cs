using Microsoft.Extensions.DependencyInjection;

namespace CoreAppHelpers.Services.Scoped;

public class Scoped<T>(T instance, IServiceScope scope) : IDisposable
{
    public T X { get; } = instance;
    private IServiceScope Scope { get; } = scope;

    public void Dispose()
    {
        Scope.Dispose();
    }
}