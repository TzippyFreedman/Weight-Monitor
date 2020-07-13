using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace MeasureService.WebApi
{
    public class Program
    {

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
               .Build();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseNServiceBus(context =>
            {

                const string endpointName = "WeightMonitor.Measure.Api";
                var endpointConfiguration = new EndpointConfiguration(endpointName);

                endpointConfiguration.EnableInstallers();

               // endpointConfiguration.AuditProcessedMessagesTo("audit");

                /*                endpointConfiguration.AuditSagaStateChanges(
                          serviceControlQueue: "Particular.Servicecontrol");*/




                var recoverability = endpointConfiguration.Recoverability();

                var outboxSettings = endpointConfiguration.EnableOutbox();

                outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
                outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));

                var auditQueue = Configuration["AppSettings:auditQueue"];
                //var auditQueue = appSettings.Get("auditQueue");
                // var serviceControlQueue = appSettings.Get("ServiceControlQueue");
                var serviceControlQueue = Configuration["AppSettings:ServiceControlQueue"];
                var measureEndpoint = Configuration["AppSettings:MeasureEndpoint"];

                var timeToBeReceivedSetting = Configuration["AppSettings:timeToBeReceived"];
                var subscriberEndpoint = Configuration["AppSettings:SubscriberEndpoint"];
                var transportConnection = Configuration.GetConnectionString("transportConnection");
                //var timeToBeReceivedSetting = appSettings.Get("timeToBeReceived");
                var timeToBeReceived = TimeSpan.Parse(timeToBeReceivedSetting);
                endpointConfiguration.AuditProcessedMessagesTo(
                    auditQueue: auditQueue,
                    timeToBeReceived: timeToBeReceived);


                recoverability.Delayed(
                    customizations: delayed =>
                    {
                        delayed.NumberOfRetries(0);
                        delayed.TimeIncrease(TimeSpan.FromMinutes(4));
                    });

                recoverability.Immediate(
                    customizations: immidiate =>
                    {
                        immidiate.NumberOfRetries(1);

                    });

                var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                transport.UseConventionalRoutingTopology()
                    .ConnectionString(transportConnection);

                var routing = transport.Routing();

                routing.RouteToEndpoint(
                    messageType: typeof(IAddMeasure),
                    destination: measureEndpoint);

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                var connection = Configuration.GetConnectionString("weightMonitorSubscriberDBConnectionString");

                persistence.SqlDialect<SqlDialect.MsSqlServer>();

                persistence.ConnectionBuilder(
                    connectionBuilder: () =>
                    {
                        return new SqlConnection(connection);
                    });

                var subscriptions = persistence.SubscriptionSettings();
                subscriptions.CacheFor(TimeSpan.FromMinutes(10));

                var scanner = endpointConfiguration.AssemblyScanner();
                scanner.ScanAssembliesInNestedDirectories = true;

                endpointConfiguration.AuditSagaStateChanges(
                    serviceControlQueue: "Particular.Servicecontrol");

                var conventions = endpointConfiguration.Conventions();
                conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
                conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

                return endpointConfiguration;
            })
                 .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
