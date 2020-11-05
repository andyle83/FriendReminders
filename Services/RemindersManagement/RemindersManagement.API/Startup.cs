using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemindersManagement.API.Domain.Interfaces;
using RemindersManagement.API.Domain.Services;
using RemindersManagement.API.Infrastructure.Data;
using RemindersManagement.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using Prometheus;

namespace RemindersManagement.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });            

            services.AddDbContext<RemindersDbContext>(options => {
                options.UseSqlite("Data Source=FriendReminders.db");
            });

            services.AddScoped<IRemindersRepository, RemindersRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IRemindersService, RemindersService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register the Swagger generator
            services.AddSwaggerGen();

            // Register health check services
            services.AddHealthChecks();

            // Register HealthChecks UI and storage provider
            /*
            services
                .AddHealthChecksUI()
                .AddMySqlStorage("Server=healthcheckdbserver;uid=root;Password=password;database=healthcheckdb");
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseForwardedHeaders();
                app.UseHsts();                
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseMetricServer();
            app.UseHttpMetrics();

            // Create health check endpoint `/health`.
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // Return HealthReport data to show in HealthCheck UI
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RemindersManagement V1");
            });
        }
    }
}
