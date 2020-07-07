using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
   public interface IMeasureUpdated
    {
        Guid MeasureId { get; set; }
        Guid UserFileId { get; set; }
        int Weight { get; set; }
    }
}
