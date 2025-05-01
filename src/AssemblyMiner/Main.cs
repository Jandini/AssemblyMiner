using AssemblyMiner.Services.Version;
using Microsoft.Extensions.Logging;

internal class Main(ILogger<Main> logger, IVersionMiner versionMiner)
{

    public async Task RunAsync(string path, CancellationToken cancellationToken = default)
    {
        var version = versionMiner.GetInformationalVersion(path);

        logger.LogInformation(version);

        await Task.CompletedTask;
    }
}
