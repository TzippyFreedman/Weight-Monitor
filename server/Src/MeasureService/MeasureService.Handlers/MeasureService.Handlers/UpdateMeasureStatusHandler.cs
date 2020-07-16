﻿using MeasureService.Services;
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
        private readonly IMeasureService _measureService;

        public UpdateMeasureStatusHandler(IMeasureService measureService)
        {
            _measureService = measureService;
        }
        public async Task Handle(IUpdateMeasureStatus message, IMessageHandlerContext context)
        {

              await _measureService.UpdateStatus(message.MeasureId, message.MeasureStatus, message.Comments);

            await context.Reply<IUpdateMeasureStatusResponse>(msg =>
            {
                msg.measureStatus = message.MeasureStatus;

            });

            /*            await context.Publish<IMeasureStatusUpdated>(msg =>
                        {
                            msg.MeasureId = message.MeasureId;
                            msg.measureStatus = message.MeasureStatus;

                        });*/
        }
    }
}
