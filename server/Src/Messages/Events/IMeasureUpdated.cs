using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
   public interface IMeasureStatusUpdated
    {
        Guid MeasureId { get; set; }
    }
}
