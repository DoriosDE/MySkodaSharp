using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MySkodaSharp.CLI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var commandLine = new Arguments(args);

            var user = commandLine["user"];
            var pass = commandLine["pass"];
            var vin = commandLine["vin"];

            if (user == null || pass == null)
            {
                Console.WriteLine("Missing -user or -pass argument!");
                Console.ReadKey();
                return;
            }

            var serviceProvider = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.SetMinimumLevel(LogLevel.Trace);
                })
                .AddSingleton<MySkodaClient>()
                .BuildServiceProvider();

            try
            {
                var mySkodaClient = serviceProvider.GetService<MySkodaClient>();
                await mySkodaClient.InitializeAsync(user, pass);

                var vehicles = await mySkodaClient.GetVehiclesAsync();

                var enyaqProvider = await mySkodaClient.CreateVehicleProviderAsync(vin);
                var enyaqDetails = await enyaqProvider.GetVehicleAsync();
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetService<ILogger<MySkodaClient>>();
                logger?.LogError(ex.Message);
            }
        }
    }
}
