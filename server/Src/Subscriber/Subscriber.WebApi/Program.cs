using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport;
using Serilog;

namespace Subscriber.WebApi
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
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });
            try
            {
                Log.Information("The program has started!!!");

                CreateHostBuilder(args).Build().Run();


            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                        .UseNServiceBus(context =>
                        {

                            const string endpointName = "WeightMonitor.Subscriber";
                            var endpointConfiguration = new EndpointConfiguration(endpointName);

                            endpointConfiguration.EnableInstallers();

                            endpointConfiguration.AuditProcessedMessagesTo("audit");

                            /*                endpointConfiguration.AuditSagaStateChanges(
                                      serviceControlQueue: "Particular.Servicecontrol");*/



                            var outboxSettings = endpointConfiguration.EnableOutbox();

                            outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
                            outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));

                            SubscribeToNotifications.Subscribe(endpointConfiguration);


                            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                            transport.UseConventionalRoutingTopology()
                                .ConnectionString("host= localhost:5672");


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

                            var recoverability = endpointConfiguration.Recoverability();
                            recoverability.CustomPolicy(SubscriberServiceRetryPolicy);

                            var conventions = endpointConfiguration.Conventions();
                            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
                            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

                           
                            return endpointConfiguration;
                        })

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static RecoverabilityAction SubscriberServiceRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            if (!(action is DelayedRetry delayedRetryAction))
            {
                return action;
            }
           /* if (context.Exception is PatientNotExistExcption)
            {
                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            }*/
            // Override default delivery delay.
            return RecoverabilityAction.DelayedRetry(TimeSpan.FromMinutes(3));

        }

      
    }

    

}
