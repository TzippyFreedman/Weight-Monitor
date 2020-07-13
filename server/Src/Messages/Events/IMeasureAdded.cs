using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public interface IMeasureAdded
    {
        Guid MeasureId { get; set; }
        Guid UserFileId { get; set; }
        float Weight { get; set; }
    }
}
