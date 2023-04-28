using Hangfire;
using Hangfire.SqlServer;
using HangFireWithDotnet6;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        //Getting hangfire connection string from appsetting
        var hangFireConnectionString = configuration.GetConnectionString("HangfireConnection");

        // adding and configuring hangfire
        services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(hangFireConnectionString, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));
        //adding hangfire server
        services.AddHangfireServer();
        services.AddTransient<App>();

        // create service provider
        var serviceProvider = services.BuildServiceProvider();

        // entry point to run app
        serviceProvider.GetService<App>().Main();

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