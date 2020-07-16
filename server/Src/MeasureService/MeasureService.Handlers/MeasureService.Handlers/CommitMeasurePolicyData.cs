using Messages.Enums;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureService.Handlers
{
    public class CommitMeasurePolicyData : ContainSagaData
    {
        public Guid MeasureId { get; set; }
        public MessageStatus WeightUpdateStatus { get; set; }
        public MessageStatus EmailSendStatus { get; set; }

    }
}