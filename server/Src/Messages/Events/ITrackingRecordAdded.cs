using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public interface ITrackingRecordAdded
    {
        public Guid MeasureId { get; set; }
    }
}
