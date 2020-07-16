using Messages.Commands;
using Messages.Enums;
using Messages.Events;
using Messages.Messages;
using NServiceBus;
using Subscriber.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Handlers
{
    class UpdateWeightHandler : IHandleMessages<IUpdateWeight>
    {
        private readonly IUserService _userService;

        public UpdateWeightHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task Handle(IUpdateWeight message, IMessageHandlerContext context)
        {

            bool isUpdateSuccess =    await _userService.UpdateWeight(message.UserFileId, message.Weight);


            await context.Reply<IUpdateWeightResponse>(msg =>
            {
                msg.status = isUpdateSuccess ? MessageStatus.Succeeded : MessageStatus.Failed;
            });

            /*            await context.Publish<IWeightUpdated>(msg =>
                        {
                            msg.MeasureId = message.MeasureId;
                            msg.status = isUpdateSuccess ? MessageStatus.Succeeded : MessageStatus.Failed;
                        });*/
        }
    }
}
