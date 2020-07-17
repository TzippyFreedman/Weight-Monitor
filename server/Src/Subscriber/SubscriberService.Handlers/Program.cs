using AutoMapper;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport;
using Subscriber.Data;
using Subscriber.Data.Exceptions;
using Subscriber.Services;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SubscriberService.Handlers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string endpointName = "WeightMonitor.Subscriber";

            Console.Title = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            //if in development
            endpointConfiguration.PurgeOnStartup(true);

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            containerSettings.ServiceCollection.AddScoped(typeof(IUserService), typeof(UserService));
            containerSettings.ServiceCollection.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            containerSettings.ServiceCollection.AddAutoMapper(typeof(Program));

            containerSettings.ServiceCollection.AddDbContext<UserDbContext>
               (options => options
               .UseSqlServer(ConfigurationManager.ConnectionStrings["weightMonitorSubscriberDBConnectionString"].ToString()));

            var appSettings = ConfigurationManager.AppSettings;

            var auditQueue = appSettings.Get("auditQueue");
            var serviceControlQueue = appSettings.Get("ServiceControlQueue");
            var measureEndpoint = appSettings.Get("MeasureEndpoint");
            var trackingEndpoint = appSettings.Get("TrackingEndpoint");
            var timeToBeReceivedSetting = appSettings.Get("timeToBeReceived");


            var timeToBeReceived = TimeSpan.Parse(timeToBeReceivedSetting);
            endpointConfiguration.AuditProcessedMessagesTo(
                auditQueue: auditQueue,
                timeToBeReceived: timeToBeReceived);

            endpointConfiguration.EnableInstallers();


            /* endpointConfiguration.AuditSagaStateChanges(
                       serviceControlQueue: serviceControlQueue);*/



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



            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology()
                .ConnectionString(transportConnection);

            var routing = transport.Routing();

            routing.RouteToEndpoint(
                messageType: typeof(IAddTrackingRecord),
                destination: trackingEndpoint);


            routing.RouteToEndpoint(
                messageType: typeof(IUpdateMeasureStatus),
                destination: measureEndpoint);

            var conventions = endpointConfiguration.Conventions();

            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
            conventions.DefiningMessagesAs(type => type.Namespace == "Messages.Messages");


            var recoverability = endpointConfiguration.Recoverability();
            recoverability.CustomPolicy(SubscriberServiceRetryPolicy);

             endpointConfiguration.RegisterComponents(c =>
            {
                c.ConfigureComponent(b =>
                {
                    var session = b.Build<ISqlStorageSession>();

                    var context = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
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

        private static RecoverabilityAction SubscriberServiceRetryPolicy(RecoverabilityConfig config, ErrorContext context)
        {

            var action = DefaultRecoverabilityPolicy.Invoke(config, context);

            if (!(action is DelayedRetry delayedRetryAction))
            {
                return action;
            }
            if (context.Exception is UserNotFoundException)
            {

                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            }
            // Override default delivery delay.
            /*          var recoverability = endpointConfiguration.Recoverability();
                        recoverability.Delayed(
                            customizations: delayed =>
                            {
                                delayed.NumberOfRetries(3);
                                delayed.TimeIncrease(TimeSpan.FromMinutes(3));
                            });*/
            return RecoverabilityAction.DelayedRetry(TimeSpan.FromMinutes(3));

        }
    }

    
}

