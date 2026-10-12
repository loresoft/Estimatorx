using Spectre.Console.Cli;

namespace Estimatorx.Migration;

public sealed class TypeResolver(IServiceProvider provider) : ITypeResolver
{
    public object? Resolve(Type? type)
        => type == null ? null : provider.GetService(type);

    public void Dispose()
    {
        if (provider is IDisposable disposable)
            disposable.Dispose();
    }
}
