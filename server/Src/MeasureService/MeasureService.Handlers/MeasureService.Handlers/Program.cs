using AutoMapper;
using MeasureService.Data;
using MeasureService.Handlers.Services;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.Sql;
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
            //if in development
            endpointConfiguration.PurgeOnStartup(true);


            //endpointConfiguration.AuditProcessedMessagesTo("audit");
            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            containerSettings.ServiceCollection.AddScoped(typeof(IMeasureHandlersRepository), typeof(MeasureHandlerRepository));

            containerSettings.ServiceCollection.AddAutoMapper(typeof(Program));


           /* containerSettings.ServiceCollection.AddDbContext<MeasureDbContext>
               (options => options
               .UseSqlServer(ConfigurationManager.ConnectionStrings["weightMonitorMeasureDBConnectionString"].ToString()));*/

            using (var receiverDataContext = new MeasureDbContext(new DbContextOptionsBuilder<MeasureDbContext>()
                .UseSqlServer(new SqlConnection(ConfigurationManager.ConnectionStrings["weightMonitorMeasureDBConnectionString"].ToString()))
                .Options))
            {
                await receiverDataContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            }
            var appSettings = ConfigurationManager.AppSettings;
            var auditQueue = appSettings.Get("AuditQueue");
            var subscriberEndpoint = appSettings.Get("SubscriberEndpoint");
            var trackingEndpoint = appSettings.Get("TrackingEndpoint");

            var serviceControlQueue = appSettings.Get("ServiceControlQueue");
            var timeToBeReceivedSetting = appSettings.Get("TimeToBeReceived");
            var timeToBeReceived = TimeSpan.Parse(timeToBeReceivedSetting);
            endpointConfiguration.AuditProcessedMessagesTo(
                auditQueue: auditQueue,
                timeToBeReceived: timeToBeReceived);
/*            endpointConfiguration.AuditSagaStateChanges(
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
                    delayed.NumberOfRetries(0);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(3));
                });

            recoverability.Immediate(
               customizations: delayed =>
               {
                   delayed.NumberOfRetries(1);
                  
               });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology()
                .ConnectionString(transportConnection);

            var routing = transport.Routing();

            routing.RouteToEndpoint(
               messageType : typeof(ISendEmail),
                destination: subscriberEndpoint);

              routing.RouteToEndpoint(
               messageType : typeof(IUpdateWeight),
                destination: subscriberEndpoint);

            routing.RouteToEndpoint(
             messageType: typeof(IAddTrackingRecord),
              destination: trackingEndpoint);

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
            conventions.DefiningMessagesAs(type => type.Namespace == "Messages.Messages");


            endpointConfiguration.RegisterComponents(c =>
            {
                c.ConfigureComponent(b =>
                {
                    var session = b.Build<ISqlStorageSession>();

                    var context = new MeasureDbContext(new DbContextOptionsBuilder<MeasureDbContext>()
                        .UseSqlServer(session.Connection)
                        .Options);

                    //Use the same underlying ADO.NET transaction
                    context.Database.UseTransaction(session.Transaction);

                    //Ensure context is flushed before the transaction is committed
                    session.OnSaveChanges(s => context.SaveChangesAsync());

                    return context;


                }, DependencyLifecycle.InstancePerUnitOfWork);
            });


            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);

        }
    }
}
