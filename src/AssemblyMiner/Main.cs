using AssemblyMiner.Services.Version;
using Microsoft.Extensions.Logging;

internal class Main(ILogger<Main> logger, IVersionMiner versionMiner)
{

    public void GetVersion(string path, string variableName)
    {
        var version = versionMiner.GetInformationalVersion(path);

        if (!string.IsNullOrEmpty(variableName))
        {
            logger.LogInformation($"Setting environment variable {variableName} to {version}");
            Environment.SetEnvironmentVariable(variableName, version);
        }
        else
        {
            logger.LogWarning($"Output environment variable was not provided.");
        }
    }
}
