
using Messages.Commands;
using Messages.Enums;
using Messages.Enums.MeasureStatus;
using Messages.Events;
using Messages.Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Handlers
{
    public class CommitMeasurePolicy : Saga<CommitMeasurePolicy.CommitMeasurePolicyData>,
               IAmStartedByMessages<IMeasureAdded>,
        IHandleMessages<ISendEmailResponse>,
        IHandleMessages<IUpdateWeightResponse>,
        IHandleMessages<IUpdateMeasureStatusResponse>
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


        public async Task Handle(ISendEmailResponse message, IMessageHandlerContext context)
        {
            Data.EmailSendStatus = message.status;
            await UpdateMeasureStatus(context);
        }

        public async Task Handle(IUpdateWeightResponse message, IMessageHandlerContext context)
        {
            Data.WeightUpdateStatus = message.status;
            await UpdateMeasureStatus(context);
        }

        public async Task Handle(IUpdateMeasureStatusResponse message, IMessageHandlerContext context)
        {
            if (message.measureStatus.Equals(MeasureStatus.Fulfilled))
            {
                await context.Send<IAddTrackingRecord>(msg =>
                {
                    //Id,CardId, weight, date, trend, BMI, comments
                    msg.MeasureId = Data.MeasureId; ;
                    /*                msg.BMI = Data.BMI;
                                    msg.Weight = Data.Weight;
                                    msg.UserFileId = Data.UserFileId;
                                    msg.Comments = Data.Comments;*/

                });
                //reply to originator??
            }

            MarkAsComplete();


        }

/*        //maybe not neccessary
        public async Task Handle(IAddTrackingRecordResponse message, IMessageHandlerContext context)
        {
            MarkAsComplete();
        }*/



        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CommitMeasurePolicyData> mapper)
        {
            mapper.ConfigureMapping<IMeasureAdded>(message => message.MeasureId)
                 .ToSaga(sagaData => sagaData.MeasureId);
/*
            mapper.ConfigureMapping<IMeasureStatusUpdated>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<IEmailSent>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<IWeightUpdated>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);

            mapper.ConfigureMapping<ITrackingRecordAdded>(message => message.MeasureId)
                  .ToSaga(sagaData => sagaData.MeasureId);*/
        }

        private async Task UpdateMeasureStatus(IMessageHandlerContext context)
        {
            if (Data.EmailSendStatus!= MessageStatus.Pending && Data.WeightUpdateStatus != MessageStatus.Pending)
            {
                MeasureStatus commitMeasureStatus = (Data.EmailSendStatus == MessageStatus.Succeeded && Data.WeightUpdateStatus == MessageStatus.Succeeded) ? MeasureStatus.Fulfilled : MeasureStatus.Error;
                await context.SendLocal<IUpdateMeasureStatus>(msg =>
                {
                    msg.MeasureId = Data.MeasureId;
                    msg.MeasureStatus = commitMeasureStatus;

                });

            }

        }
        public class CommitMeasurePolicyData : ContainSagaData
        {
            public Guid MeasureId { get; set; }
            public MessageStatus WeightUpdateStatus { get; set; }
            public MessageStatus EmailSendStatus { get; set; }

        }

    }
}
