using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public interface IAddMeasure
    {
        public Guid MeasureId { get; set; }
        Guid UserFileId { get; set; }
        public float Weight { get; set; }


    }
}
