using System;
using System.Collections.Generic;
using System.Text;

namespace TrackingService.Services.Models
{
   public class UserRecordModel
    {
        public DateTime Date { get; set; }
        public Guid UserFileId { get; set; }
        public float Weight { get; set; }
        public string Comments { get; set; }
        public float BMI { get; set; }

        public float Trend { get; set; }
    }
}
