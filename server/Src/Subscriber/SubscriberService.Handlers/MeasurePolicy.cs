
using Messages.Commands;
using Messages.Enums.MeasureStatus;
using Messages.Events;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NServiceBus;
using Subscriber.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Handlers
{
    public class MeasurePolicy : Saga<MeasureDataPolicy>,
               IAmStartedByMessages<IUpdateUserFile>,
        IHandleMessages<IEmailSent>,
        IHandleMessages<IWeightUpdated>,
        IHandleMessages<IMeasureStatusUpdated>,
        IHandleMessages<ITrackingRecordAdded>
    {

        private readonly IUserService _userService;
        public MeasurePolicy(IUserService userService)
        {
            _userService = userService;
        }
        public async Task Handle(IUpdateUserFile message, IMessageHandlerContext context)
        {
            //command send to local handler updatebmi
            Data.Weight = message.Weight;
            Data.UserFileId = message.UserFileId;
            await context.SendLocal<IUpdateWeight>(msg =>
             {
                 msg.MeasureId = message.MeasureId;
                 msg.UserFileId = message.UserFileId;
                 msg.Weight = message.Weight;
             });

            await context.SendLocal<ISendEmail>(msg =>
            {
                msg.UserFileId = message.UserFileId;
                msg.MeasureId = message.MeasureId;
                msg.Weight = message.Weight;
            });
        }

        public async Task Handle(IEmailSent message, IMessageHandlerContext context)
        {
            Data.IsEmailSent = true;
            await NotifyMeasureService(context);
        }

        public async Task Handle(IWeightUpdated message, IMessageHandlerContext context)
        {
            Data.BMI = message.BMI;
            Data.IsWeightUpdated = true;
            await NotifyMeasureService(context);
        }

        public async Task Handle(IMeasureStatusUpdated message, IMessageHandlerContext context)
        {
            await context.Send<IAddTrackingRecord>(msg =>
            {
                //Id,CardId, weight, date, trend, BMI, comments
                msg.MeasureId = message.MeasureId;
                msg.BMI = Data.BMI;
                msg.Weight = Data.Weight;
                msg.UserFileId = Data.UserFileId;
                msg.Comments = Data.Comments;

            });
        }

        public async Task Handle(ITrackingRecordAdded message, IMessageHandlerContext context)
        {
            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MeasureDataPolicy> mapper)
        {
            mapper.ConfigureMapping<IUpdateUserFile>(message => message.MeasureId)
                            .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<IMeasureStatusUpdated>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<IEmailSent>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<IWeightUpdated>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<ITrackingRecordAdded>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);
        }

        private async Task NotifyMeasureService(IMessageHandlerContext context)
        {
            if (Data.IsWeightUpdated && Data.IsEmailSent)
            {
                await context.Send<IUpdateMeasureStatus>(msg =>
                {
                    msg.MeasureId = Data.MeasureId;
                    msg.MeasureStatus = MeasureStatus.Fulfilled;

                });

            }

        }

    }
}
