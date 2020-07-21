using CustomerService.DAL;
using CustomerService.Interfaces;
using CustomerService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Configuration;

namespace CustomerService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            var ConnectionString = Configuration.GetConnectionString("DbConnection");
            //var connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["DbConnection"]);

            //Entity Framework  
            services.AddDbContext<CustomerServiceContext>(options => options.UseSqlServer(ConnectionString));

            services.AddScoped<IUsers, UserDAL>();
            services.AddScoped<ICustomerType, CustomerTypeDAL>();
            services.AddScoped<ICustomerDetails, CustomerDetailsDAL>();

            var serilogLogger = new LoggerConfiguration()
                   .WriteTo.File("CustomerServiceLog.txt")
                   .CreateLogger();
            services.AddLogging(x =>
            {
                x.SetMinimumLevel(LogLevel.Information);
                x.AddSerilog(logger: serilogLogger, dispose: true);
            });
            //var host = builder.Build();

            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            //app.UseAuthentication();

            //app.UseAuthorization();

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
