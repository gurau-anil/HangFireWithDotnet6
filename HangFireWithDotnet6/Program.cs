using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(@"appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        // Retrieve the configuration settings from the configuration system
        var configuration = hostContext.Configuration;

        var connectionString = configuration.GetConnectionString("DefaultConnection");

    })
    .UseConsoleLifetime();

await builder.RunConsoleAsync();