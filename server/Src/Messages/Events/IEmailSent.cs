using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public interface IEmailSent
    {
        Guid MeasureId { get; set; }
        Guid UserFileId { get; set; }
    }
}
