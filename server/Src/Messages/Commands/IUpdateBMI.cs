using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public interface IUpdateWeight
    {
        Guid UserFileId { get; set; }
        Guid MeasureId { get; set; }
        float Weight { get; set; }
    }
}
