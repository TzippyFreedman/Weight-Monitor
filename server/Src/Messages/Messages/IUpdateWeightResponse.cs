using Messages.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Messages
{
    public interface IUpdateWeightResponse
    {
        MessageStatus status { get; set; }
    }
}
