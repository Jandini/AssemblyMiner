using CommandLine;

internal sealed class Options
{
    [Verb("version", isDefault: true, HelpText = "Get assembly informational version.")]
    internal class Version
    {
        [Option('p', "path", Required = true, HelpText = "Path to the assembly file.")]
        public string Path { get; set; }
    }
}
