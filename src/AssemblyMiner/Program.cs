// Created with JandaBox 0.9.4 http://github.com/Jandini/JandaBox
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CommandLine;

Parser.Default.ParseArguments<Options.Version>(args).WithParsed((parameters) =>
{

    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddApplicationSettings()
        .Build();

    using var serviceProvider = new ServiceCollection()
        .AddConfiguration(config)
        .AddLogging(config)
        .AddServices()
        .BuildServiceProvider();

    serviceProvider.LogVersion<Program>();

    try
    {
        var main = serviceProvider.GetRequiredService<Main>();

        switch (parameters)
        {
            case Options.Version options:
                main.GetVersion(options.Path, options.EnvironmentVariableName);
                break;
        };
    }
    catch (Exception ex)
    {
        serviceProvider.GetService<ILogger<Program>>()?
            .LogCritical(ex, "Program failed.");
    }
});
