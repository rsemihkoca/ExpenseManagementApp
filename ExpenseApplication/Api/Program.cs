namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        // Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
        // Log.Information("App server is starting.");
        
        Host.CreateDefaultBuilder(args)
            // .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).Build().Run();
    }
}