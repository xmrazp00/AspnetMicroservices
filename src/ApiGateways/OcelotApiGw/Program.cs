using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotApiGw
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOcelot()
                .AddCacheManager(opt =>
                {
                    opt.WithDictionaryHandle();
                });

            builder.Host
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.json", true, true);
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            await app.UseOcelot();

            await app.RunAsync();
        }
    }
}
