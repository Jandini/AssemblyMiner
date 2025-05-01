// Created with JandaBox 0.9.4 http://github.com/Jandini/JandaBox
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CommandLine;

await Parser.Default.ParseArguments<Options.Version>(args).WithParsedAsync(async (parameters) =>
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
        using var cancellationTokenSource = serviceProvider.GetCancellationTokenSource();
        var main = serviceProvider.GetRequiredService<Main>();

        switch (parameters)
        {
            case Options.Version options:
                await main.RunAsync(options.Path, cancellationTokenSource.Token);
                break;
        };
    }
    catch (Exception ex)
    {
        serviceProvider.GetService<ILogger<Program>>()?
            .LogCritical(ex, "Program failed.");
    }
});
