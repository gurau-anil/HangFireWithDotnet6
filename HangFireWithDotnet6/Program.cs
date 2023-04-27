using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(getProjectRootDirectory())
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

string getProjectRootDirectory()
{
    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string rootDirectory = baseDirectory;

    while (Directory.GetParent(rootDirectory) != null && !File.Exists(Path.Combine(rootDirectory, "Program.cs")))
    {
        rootDirectory = Directory.GetParent(rootDirectory).FullName;
    }
    return rootDirectory;
}