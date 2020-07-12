using Messages.Commands;
using Messages.Events;
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

            float updatedBMI =    await _userService.UpdateWeight(message.UserFileId, message.Weight);


            await context.Publish<IWeightUpdated>(msg =>
            {
                msg.MeasureId = message.MeasureId;
                msg.BMI = updatedBMI;
            });
        }
    }
}
