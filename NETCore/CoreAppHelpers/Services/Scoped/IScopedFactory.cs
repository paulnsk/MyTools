namespace CoreAppHelpers.Services.Scoped;

public interface IScopedFactory<T>
{
    public Scoped<T> Create();
}