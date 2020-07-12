using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public interface IUpdateMeasureStatus
    {
        public Guid MeasureId { get; set; }

        public MeasureStatus MeasureStatus { get; set; }
        public string Comments { get; set; }
    }
}
