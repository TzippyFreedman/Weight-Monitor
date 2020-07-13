using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
   public interface IMeasureStatusUpdated
    {
        Guid MeasureId { get; set; }
        MeasureStatus measureStatus { get; set; }
    }
}
