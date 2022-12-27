using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Instrument
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            ConfigureServices(out ServiceProvider services);

            var loginForm = services.GetService<LoginForm>();
            Application.Run(loginForm);
        }

        private static void ConfigureServices(out ServiceProvider sp)
        {
            var services = new ServiceCollection();
            
            services.AddScoped<LoginForm>();
            services.AddScoped<InstrumentForm>();


            #region Configuration

            // IConfiguration configuration =
            //     new ConfigurationBuilder()
            //         .SetBasePath(Directory.GetCurrentDirectory())
            //         .AddJsonFile("appsettings.json", optional: true,
            //             reloadOnChange: true).Build();
            IConfiguration configuration = new ConfigurationBuilder()
                .Add<WritableJsonConfigurationSource>(s =>
                {
                    s.Path ="appsettings.json";
                    s.Optional = true;
                    s.ReloadOnChange = true;
                    s.ResolveFileProvider();
                })
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            
            #endregion


            #region Logging

            var serilogger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(serilogger));

            #endregion

            sp = services.BuildServiceProvider();
        }
    }
}