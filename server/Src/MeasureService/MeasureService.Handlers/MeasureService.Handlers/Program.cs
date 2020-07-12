using AutoMapper;
using MeasureService.Data;
using MeasureService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MeasureService.Handlers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string endpointName = "WeightMonitor.Measure";

            Console.Title = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration.EnableInstallers();

            //endpointConfiguration.AuditProcessedMessagesTo("audit");
            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            containerSettings.ServiceCollection.AddScoped(typeof(IMeasureService), typeof(MeasureService.Services.MeasureService));
            containerSettings.ServiceCollection.AddScoped(typeof(IMeasureRepository), typeof(MeasureRepository));

            containerSettings.ServiceCollection.AddAutoMapper(typeof(Program));


            containerSettings.ServiceCollection.AddDbContext<MeasureDbContext>
               (options => options
               .UseSqlServer(ConfigurationManager.ConnectionStrings["weightMonitorMeasureDBConnectionString"].ToString()));

            var appSettings = ConfigurationManager.AppSettings;
            var auditQueue = appSettings.Get("auditQueue");
            var serviceControlQueue = appSettings.Get("ServiceControlQueue");
            var timeToBeReceivedSetting = appSettings.Get("timeToBeReceived");
            var timeToBeReceived = TimeSpan.Parse(timeToBeReceivedSetting);
            endpointConfiguration.AuditProcessedMessagesTo(
                auditQueue: auditQueue,
                timeToBeReceived: timeToBeReceived);
            /* endpointConfiguration.AuditSagaStateChanges(
                       serviceControlQueue: "Particular.Servicecontrol");*/



            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            var persistenceConnection = ConfigurationManager.ConnectionStrings["persistenceConnection"].ToString();

            var transportConnection = ConfigurationManager.ConnectionStrings["transportConnection"].ToString();


            persistence.SqlDialect<SqlDialect.MsSqlServer>();

            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(persistenceConnection);
                });

            var outboxSettings = endpointConfiguration.EnableOutbox();


            outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
            outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));


            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(10));

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(3);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(3));
                });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology()
                .ConnectionString(transportConnection);

            var routing = transport.Routing();

/*            routing.RouteToEndpoint(
                assembly: typeof(INotifyPolice).Assembly,
                destination: "Police");*/

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);

        }
    }
}
