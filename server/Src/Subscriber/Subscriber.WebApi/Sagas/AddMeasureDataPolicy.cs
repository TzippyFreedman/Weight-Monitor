using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.WebApi.Sagas
{
    public class AddMeasureDataPolicy : ContainSagaData
    {
        public Guid MeasureId { get; set; }
        public Guid UserFileId { get; set; }
        public int Weight { get; set; }
    }
}
