using MeasureService.Services;
using Messages.Commands;
using Messages.Enums;
using Messages.Events;
using Messages.Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Handlers
{
    class UpdateMeasureStatusHandler : IHandleMessages<IUpdateMeasureStatus>
    {
        private readonly IMeasureHandlerRepository _measureRopository;

        public UpdateMeasureStatusHandler(IMeasureHandlerRepository measureRopository)
        {
            _measureRopository = measureRopository;
        }
        public async Task Handle(IUpdateMeasureStatus message, IMessageHandlerContext context)
        {
            await _measureRopository.UpdateStatusAsync(message.MeasureId, message.MeasureStatus, message.Comments);
             // await _measureService.UpdateStatus(message.MeasureId, message.MeasureStatus, message.Comments);

            await context.Reply<IUpdateMeasureStatusResponse>(msg =>
            {
                msg.measureStatus = message.MeasureStatus;

            });


        }
    }
}
