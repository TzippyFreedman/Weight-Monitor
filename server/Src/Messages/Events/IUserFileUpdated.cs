using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;


namespace Messages.Events
{
    public interface IUserFileUpdated
    {
         Guid MeasureId { get; set; }

         MeasureStatus Status { get; set; }
         string Comments { get; set; }
    }
}
