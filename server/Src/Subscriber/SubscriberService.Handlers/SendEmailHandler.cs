using Messages.Commands;
using Messages.Enums;
using Messages.Events;
using Messages.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Handlers
{
    class SendEmailHandler : IHandleMessages<ISendEmail>
    {

        static ILog log = LogManager.GetLogger<ISendEmail>();
        public Task Handle(ISendEmail message, IMessageHandlerContext context)
        {
            //  log.Info($"Received PatientCreated, PatientId = {message.}");

            return context.Reply<ISendEmailResponse>(msg =>
            {
                msg.status = MessageStatus.Succeeded;
            });

/*            return context.Publish<IEmailSent>(msg =>
            {
                msg.MeasureId = message.MeasureId;
                msg.status = RequestStatus.Succeeded;
            });*/


        }
    }
}
