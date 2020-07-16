using Messages.Enums;
using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Messages
{
    public interface IUpdateMeasureStatusResponse
    {
        MeasureStatus measureStatus { get; set; }
    }
}
