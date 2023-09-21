using Microsoft.Extensions.DependencyInjection;

namespace CoreApplication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreProviders(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IMemoryProvider<>), typeof(MemoryProvider<>));
        services.AddSingleton<IOperandProvider, OperandProvider>();
        services.AddSingleton<IOperationProvider, OperationProvider>();
        return services;
    }
}
