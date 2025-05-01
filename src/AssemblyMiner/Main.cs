using AssemblyMiner.Services.Version;
using Microsoft.Extensions.Logging;

internal class Main(ILogger<Main> logger, IVersionMiner versionMiner)
{

    public void GetVersion(string path)
    {
        var version = versionMiner.GetInformationalVersion(path);
        logger.LogInformation(version);       
    }
}
