using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackingService.Data;
using TrackingService.Data.Entities;

namespace TrackingService.Handlers
{
    public class AddTrackingRecordHandler : IHandleMessages<IAddTrackingRecord>
    {
        private readonly RecordDbContext _recordDbContext;

        public AddTrackingRecordHandler(RecordDbContext recordDbContext)
        {
            _recordDbContext = recordDbContext;
        }
        public async Task Handle(IAddTrackingRecord message, IMessageHandlerContext context)
        {
            Record record = new Record { BMI = message.BMI, Weight = message.Weight, Comments = message.Comments, UserFileId = message.UserFileId };
            float trend;
            float previousBMI;

            Record previousRecord = await _recordDbContext.Records.Where(record => record.UserFileId == message.UserFileId)?
                .OrderByDescending(record => record.Date)?
                .FirstOrDefaultAsync();

            if (previousRecord != null)
            {
                previousBMI = previousRecord.BMI;
            }
            else
            {
                previousBMI = 0;
            }

            if (previousBMI != 0)
            {
                trend = Math.Abs(previousBMI - record.BMI);
            }
            else
            {
                trend = 0;
            }

            record.Trend = trend;

        }
    }
}
