using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TrackingService.Data;
using TrackingService.Services;

namespace TrackingService.Handlers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string endpointName = "WeightMonitor.Tracking";

            Console.Title = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration.EnableInstallers();
            //if in development
            endpointConfiguration.PurgeOnStartup(true);

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddScoped(typeof(ITrackingService), typeof(Services.TrackingService));
            containerSettings.ServiceCollection.AddScoped(typeof(ITrackingRepository), typeof(TrackingRepository));

            containerSettings.ServiceCollection.AddAutoMapper(typeof(Program));

            using (var recordDbContext = new RecordDbContext(new DbContextOptionsBuilder<RecordDbContext>()
          .UseSqlServer(new SqlConnection((ConfigurationManager.ConnectionStrings["weightMonitorTrackingDBConnectionString"].ToString())))
          .Options))
            {
                await recordDbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            }

            /*  containerSettings.ServiceCollection.AddDbContext<RecordDbContext>
                 (options => options
                      .UseSqlServer(ConfigurationManager.ConnectionStrings["weightMonitorTrackingDBConnectionString"].ToString()));*/

            /* endpointConfiguration.AuditSagaStateChanges(
                       serviceControlQueue: "Particular.Servicecontrol");*/

            var appSettings = ConfigurationManager.AppSettings;
            var auditQueue = appSettings.Get("AuditQueue");
            var serviceControlQueue = appSettings.Get("ServiceControlQueue");
            var timeToBeReceivedSetting = appSettings.Get("timeToBeReceived");
            var timeToBeReceived = TimeSpan.Parse(timeToBeReceivedSetting);
            endpointConfiguration.AuditProcessedMessagesTo(
                auditQueue: auditQueue,
                timeToBeReceived: timeToBeReceived);



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


            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
            conventions.DefiningMessagesAs(type => type.Namespace == "Messages.Messages");

            endpointConfiguration.RegisterComponents(c =>
            {
                c.ConfigureComponent(b =>
                {
                    var session = b.Build<ISqlStorageSession>();

                    var context = new RecordDbContext(new DbContextOptionsBuilder<RecordDbContext>()
                        .UseSqlServer(session.Connection)
                        .Options);

                    //Use the same underlying ADO.NET transaction
                    context.Database.UseTransaction(session.Transaction);

                    //Ensure context is flushed before the transaction is committed
                    session.OnSaveChanges(s => 
                    context.SaveChangesAsync());

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
