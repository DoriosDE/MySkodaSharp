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

            var email = commandLine["email"];
            var pass = commandLine["pass"];
            var vin = commandLine["vin"];

            if (email == null || pass == null)
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
                var mySkoda = serviceProvider.GetService<MySkodaClient>();

                await mySkoda.InitializeAsync(email, pass);

                var vehicles = await mySkoda.GetGarageEntriesAsync();
                var user = await mySkoda.GetUserAsync();
                var info = await mySkoda.GetInfoAsync(vin);
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetService<ILogger<MySkodaClient>>();
                logger?.LogError(ex.Message);
            }
        }
    }
}
