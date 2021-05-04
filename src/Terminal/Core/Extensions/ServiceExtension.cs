using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Terminal.Core.Clients;
using Terminal.Core.Generators;
using Terminal.Core.Managers;
using Terminal.Infrastructure.Storage;

namespace Terminal.Core.Extensions
{
    /// <summary>
    /// Extension to build service container
    /// </summary>
    internal static class ServiceExtension
    {
        public static IServiceCollection Configure(this IServiceCollection serviceCollection)
        {
            AddApplicationServices(serviceCollection);

            serviceCollection.AddMemoryCache();
            serviceCollection
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            return serviceCollection;
        }

        private static void AddApplicationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<Application>();
            serviceCollection.AddSingleton<IStorage, Memory>();
            serviceCollection.AddSingleton<IPrinterGenerator, PrinterGenerator>();
            serviceCollection.AddSingleton<PaymentGateway>();

            serviceCollection.AddScoped<TransactionManager>();
            serviceCollection.AddScoped<AccountManager>();
            serviceCollection.AddScoped<ProcessorManager>();
        }
    }
}
