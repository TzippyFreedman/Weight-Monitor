using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Services.Models;

namespace TrackingService.Services
{
    public interface ITrackingService
    {
         Task<List<UserRecordModel>> GetRecordsByUserFileId(Guid userFileId);
        Task AddTrackingRecord(UserRecordModel record);
    }
}
