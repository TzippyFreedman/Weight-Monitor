using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
   public interface IUpdateWeight
    {
        Guid MeasureId { get; set; }
        Guid UserFileId { get; set; }
        float Weight { get; set; }
    }
}
