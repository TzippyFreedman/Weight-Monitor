using Messages.Commands;
using Messages.Events;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Data;
using TrackingService.Data.Entities;
using TrackingService.Services;
using TrackingService.Services.Models;

namespace TrackingService.Handlers
{
    public class AddTrackingRecordHandler : IHandleMessages<IAddTrackingRecord>
    {
        private readonly ITrackingService _trackingService;
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
                previousBMI= previousRecord.BMI;
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

            //Record record = _mapper.Map<Record>(newRecord);
            _recordDbContext.Records.Add(record);
            await _recordDbContext.SaveChangesAsync();
            //await _trackingRepository.Add(record);
            //await _trackingService.AddTrackingRecord(record);

            //throw new Exception();
            await context.Publish<ITrackingRecordAdded>(msg =>
             {
                 msg.MeasureId = message.MeasureId;
             });


        }
    }
}
