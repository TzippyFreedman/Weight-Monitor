using Messages.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public interface IEmailSent
    {
        Guid MeasureId { get; set; }
        public MessageStatus status { get; set; }
    }
}
