using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Services.Models;

namespace TrackingService.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly IMapper _mapper;

        public TrackingService( ITrackingRepository trackingRepository, IMapper mapper)
        {
            _trackingRepository = trackingRepository;
            _mapper = mapper;
        }

        public async Task AddTrackingRecord(UserRecordModel record)
        {
           float trend;
           float previousBMI = await _trackingRepository.GetPreviousBMI(record.UserFileId);

            if (previousBMI != 0)
            {
                trend = Math.Abs(previousBMI - record.BMI);
            }
            else
            {
                trend = 0;
            }

            record.Trend = trend;
            await _trackingRepository.Add(record);
        }

        public async Task<List<UserRecordModel>> GetRecordsByUserFileId(Guid userFileId)
        {
            return await _trackingRepository.GetRecordsByUserFileId(userFileId);
        }
    }
}
