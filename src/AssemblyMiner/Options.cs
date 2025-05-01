using CommandLine;

internal sealed class Options
{
    [Verb("version", isDefault: true, HelpText = "Get assembly informational version and set environment variable.")]
    internal class Version
    {
        [Option('p', "path", Required = true, HelpText = "Path to the assembly file.")]
        public string Path { get; set; }


        [Option('e', "env-var-name", HelpText = "Environment variable name to store the version.")]
        public string EnvironmentVariableName { get; set; }

    }
}
