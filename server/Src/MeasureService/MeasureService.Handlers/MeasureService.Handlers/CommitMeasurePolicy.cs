
using Messages.Commands;
using Messages.Enums;
using Messages.Enums.MeasureStatus;
using Messages.Events;
using NServiceBus;
using SubscriberService.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Handlers
{
    public class CommitMeasurePolicy : Saga<CommitMeasurePolicyData>,
               IAmStartedByMessages<IMeasureAdded>,
        IHandleMessages<IEmailSent>,
        IHandleMessages<IWeightUpdated>,
        IHandleMessages<IMeasureStatusUpdated>,
        IHandleMessages<ITrackingRecordAdded>
    {
        public async Task Handle(IMeasureAdded message, IMessageHandlerContext context)
        {
            await context.Send<IUpdateWeight>(msg =>
            {
                msg.MeasureId = message.MeasureId;
                msg.UserFileId = message.UserFileId;
                msg.Weight = message.Weight;
            });

            await context.Send<ISendEmail>(msg =>
            {
                msg.MeasureId = message.MeasureId;
                msg.UserFileId = message.UserFileId;
                msg.Weight = message.Weight;
            });
        }


        public async Task Handle(IEmailSent message, IMessageHandlerContext context)
        {
            Data.EmailSendStatus = message.status;
            await UpdateMeasureStatus(context);
        }

        public async Task Handle(IWeightUpdated message, IMessageHandlerContext context)
        {
            Data.WeightUpdateStatus = message.status;
            await UpdateMeasureStatus(context);
        }

        public async Task Handle(IMeasureStatusUpdated message, IMessageHandlerContext context)
        {
            if (message.measureStatus == MeasureStatus.Fulfilled)
            {
                await context.Send<IAddTrackingRecord>(msg =>
                {
                    //Id,CardId, weight, date, trend, BMI, comments
                    msg.MeasureId = message.MeasureId;
                    /*                msg.BMI = Data.BMI;
                                    msg.Weight = Data.Weight;
                                    msg.UserFileId = Data.UserFileId;
                                    msg.Comments = Data.Comments;*/

                });
            }
            else
            {
                MarkAsComplete();
            }

        }

        public async Task Handle(ITrackingRecordAdded message, IMessageHandlerContext context)
        {
            MarkAsComplete();
        }



        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CommitMeasurePolicyData> mapper)
        {
            mapper.ConfigureMapping<IMeasureAdded>(message => message.MeasureId)
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

        private async Task UpdateMeasureStatus(IMessageHandlerContext context)
        {
            if (Data.EmailSendStatus!= RequestStatus.Pending && Data.WeightUpdateStatus != RequestStatus.Pending)
            {
                MeasureStatus commitMeasureStatus = (Data.EmailSendStatus == RequestStatus.Succeeded && Data.WeightUpdateStatus == RequestStatus.Succeeded) ? MeasureStatus.Fulfilled : MeasureStatus.Error;
                await context.SendLocal<IUpdateMeasureStatus>(msg =>
                {
                    msg.MeasureId = Data.MeasureId;
                    msg.MeasureStatus = commitMeasureStatus;

                });

            }

        }

    }
}
