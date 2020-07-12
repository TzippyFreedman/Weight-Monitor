using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriberService.Handlers
{
    public class MeasureDataPolicy : ContainSagaData
    {
        public Guid MeasureId { get; set; }
        public bool IsWeightUpdated { get; set; }
        public bool IsEmailSent { get; set; }
        public float BMI { get; set; }
        public float Weight { get; set; }
        public Guid UserFileId { get; set; }
        public string Comments { get; set; }
    }
}
