namespace AssemblyMiner.Services.Version;

public interface IVersionMiner
{
    string GetInformationalVersion(string path);
}