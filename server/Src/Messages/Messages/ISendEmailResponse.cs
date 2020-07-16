using Messages.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Messages
{
    public interface ISendEmailResponse
    {
        MessageStatus status { get; set; }
    }
}
