using Messages.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
   public interface IWeightUpdated
    {
        Guid MeasureId { get; set; }
        float BMI { get; set; }
        public RequestStatus status { get; set; }
    }
}
