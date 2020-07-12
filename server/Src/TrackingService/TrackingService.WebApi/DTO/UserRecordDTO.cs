using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingService.WebApi.DTO
{ 
    public class UserRecordDTO
    {
        public DateTime Date { get; set; }
        public float Weigh { get; set; }
        public string Comments { get; set; }
        public float BMI { get; set; }

        public bool Trend { get; set; }
    }
}
