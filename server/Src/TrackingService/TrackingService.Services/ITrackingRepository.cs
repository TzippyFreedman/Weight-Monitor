using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackingService.Services.Models;

namespace TrackingService.Services
{
    public interface ITrackingRepository
    {
      Task<List<UserRecordModel>> GetRecordsByUserFileId(Guid userFileId);

        Task Add(UserRecordModel newTrackingRecord);
        Task<float> GetPreviousBMI(Guid userFileId);
    }
}
