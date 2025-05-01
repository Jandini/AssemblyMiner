using Microsoft.Extensions.DependencyInjection;

namespace AssemblyMiner.Services.Version;

public static class VersionMinerExtensions
{
    public static IServiceCollection AddVersionMiner(this IServiceCollection services)
    {
        return services.AddTransient<IVersionMiner, VersionMiner>();
    }
}