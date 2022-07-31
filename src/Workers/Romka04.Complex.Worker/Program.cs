using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Romka04.Complex.Worker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
               //     builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddOptions<RedisOptions>()
                        .Bind(context.Configuration.GetSection(RedisOptions.Name))
                        ;
                })
                .Build();

            await host.RunAsync();
        }
    }
}