using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public interface ICheckUserFileExist
    {
        public Guid MeasureId { get; set; }
        public Guid FileId { get; set; }
    }
}
