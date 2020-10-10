using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Elsa.Activities.Email.Extensions;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Timers.Extensions;
using Elsa.Activities.MassTransit;
using Elsa.Dashboard.Extensions;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Elsa.Persistence.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Elsa.Activities.MassTransit.Extensions;
using GreenPipes;
using Workflow.Events.FirstCall;
using Elsa.Activities.Console.Extensions;
using Workflow.Dashboard.Consumers;
using Workflow.Dashboard.DashboardQueries;
using Workflow.Events.Intake;

namespace Workflow.Dashboard
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionstring = Configuration.GetConnectionString("SqlServer");
            var dashboardQueries = new DashboardQueries.DashboardQueries(connectionstring);
            
            services
                .AddSingleton<IDashboardQueries>(dashboardQueries)
                .AddCustomMassTransit(Configuration)
                .AddCustomElsa(Configuration);
            // Add services used for the workflows runtime.
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                //loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHttpActivities();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseWelcomePage();
        }
    }

    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddCustomMassTransit (this IServiceCollection services, IConfiguration configuration)
        {
            void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            {
                var applicationServiceName = "metropolitan.workflow.services";
                var applicationStateName = "metropolitan.workflow.states";
                configurator.AddBus(serviceProvider =>
                {

                    var connectionString = configuration["EventBusConnection"]; //.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri(connectionString), h => {
                            h.Username("guest");
                            h.Password("guest");
                        });

                        cfg.ReceiveEndpoint(applicationStateName, ep =>
                        {
                            ep.PrefetchCount = 16;
                            ep.UseMessageRetry (r=> r.Interval(2, 100));
                            ep.ConfigureWorkflowConsumer<MorgueEntryCreated>(serviceProvider);
                            ep.ConfigureWorkflowConsumer<MorgueEntryStatusChanged>(serviceProvider);
                            ep.ConfigureWorkflowConsumer<FirstCallCreated>(serviceProvider);
                        });

                        cfg.ReceiveEndpoint(applicationServiceName, ep =>
                        {
                            ep.PrefetchCount = 16;
                            ep.UseMessageRetry(r => r.Interval(2, 100));
                            ep.ConfigureWorkflowConsumer<ChangeMorgueEntryStatus>(serviceProvider);
                            ep.ConfigureConsumer<ResetWorkflowDatabaseServiceConsumer>(serviceProvider);

                        });

                    });

                    return bus;

                });

                configurator.AddWorkflowConsumer<MorgueEntryCreated>();
                configurator.AddWorkflowConsumer<MorgueEntryStatusChanged>();
                configurator.AddWorkflowConsumer<ChangeMorgueEntryStatus>();
                configurator.AddWorkflowConsumer<FirstCallCreated>();
                configurator.AddConsumer<ResetWorkflowDatabaseServiceConsumer>();
            }

            services.AddMassTransit(ConfigureMassTransit);
            services.AddMassTransitHostedService();
            return services;
        }


        public static IServiceCollection AddCustomElsa(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionstring = configuration.GetConnectionString("SqlServer");

            
            services
                         // Add services used for the workflows runtime.
                         .AddElsa(elsa => elsa.AddEntityFrameworkStores<SqlServerContext>(options => options.UseSqlServer(connectionstring)))
                         .AddHttpActivities(options => options.Bind(configuration.GetSection("Elsa:Http")))
                         .AddEmailActivities(options => options.Bind(configuration.GetSection("Elsa:Smtp")))
                         .AddTimerActivities(options => options.Bind(configuration.GetSection("Elsa:Timers")))
                         .AddConsoleActivities()
                         .AddMassTransitActivities()
                         // Add services used for the workflows dashboard.
                         .AddElsaDashboard();
            return services;
        }
    }
}
