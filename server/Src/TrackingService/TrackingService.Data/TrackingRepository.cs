using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Data.Entities;
using TrackingService.Services;
using TrackingService.Services.Models;

namespace TrackingService.Data
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly RecordDbContext _recordDbContext;
        private readonly IMapper _mapper;

        public TrackingRepository(RecordDbContext recordDbContext, IMapper mapper)
        {
            _recordDbContext = recordDbContext;
            _mapper = mapper;
        }

        public async Task Add(UserRecordModel newRecord)
        {
            Record record = _mapper.Map<Record>(newRecord);
            _recordDbContext.Records.Add(record);
            await _recordDbContext.SaveChangesAsync();
        }

        public async Task<float> GetPreviousBMI(Guid userFileId)
        {
          Record previousRecord = await  _recordDbContext.Records.Where(record => record.UserFileId == userFileId)?
                .OrderByDescending(record => record.Date)?
                .FirstOrDefaultAsync();

            if (previousRecord != null)
            {
                return previousRecord.BMI;
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<UserRecordModel>> GetRecordsByUserFileId(Guid userFileId)
        {
            List<Record> records = await _recordDbContext.Records
                  .Where(record => record.UserFileId == userFileId)
                  .ToListAsync();
            return _mapper.Map<List<UserRecordModel>>(records);
        }
    }
}
