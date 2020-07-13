using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureService.Data.Entities
{
    public class Measure
    {
        public Guid Id { get; set; }
        public Guid UserFileId { get; set; }
        public float Weight { get; set; }
        public DateTime Date { get; set; }
        public MeasureStatus Status { get; set; }
        public string Comments { get; set; }
    }
}
