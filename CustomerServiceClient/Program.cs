using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Forms;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CustomerServiceClient
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddScoped<Form1>();

                   //Add Serilog
                   var serilogLogger = new LoggerConfiguration()
                   .WriteTo.File("CustomerClientLog.txt")
                   .CreateLogger();
                   services.AddLogging(x =>
                   {
                       x.SetMinimumLevel(LogLevel.Information);
                       x.AddSerilog(logger: serilogLogger, dispose: true);
                   });
               });

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var form1 = services.GetRequiredService<Form1>();
                    Application.Run(form1);
                }
                catch (Exception ex)
                {
                   
                }
            }
        }
    }
}
