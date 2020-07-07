using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasureService.WebApi.DTO
{
    public class MeasureDTO
    {
        public Guid UserFileId { get; set; }
        public int Weight { get; set; }
    }
}
