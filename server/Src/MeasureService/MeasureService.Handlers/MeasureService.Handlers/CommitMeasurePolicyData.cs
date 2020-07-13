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
        public RequestStatus WeightUpdateStatus { get; set; }
        public RequestStatus EmailSendStatus { get; set; }

    }
}