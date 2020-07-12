using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public interface IAddTrackingRecord
    {
        public Guid MeasureId { get; set; }

        public Guid UserFileId { get; set; }
        public float Weight { get; set; }
        //   public DateTime Date { get; set; }

        public float BMI { get; set; }
        public string Comments { get; set; }
       // Id,CardId, weight, date, trend, BMI, comments
    }
}
