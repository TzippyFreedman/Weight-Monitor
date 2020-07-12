using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
   public interface IWeightUpdated
    {
        Guid MeasureId { get; set; }
        Guid UserFileId { get; set; }
        float BMI { get; set; }
    }
}
