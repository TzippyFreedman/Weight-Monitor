using MeasureService.Services.Models;
using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Services
{
    public interface IMeasureRepository
    {
        Task UpdateStatusAsync(Guid measureId, MeasureStatus status, string comments);
        Task<MeasureModel> Add(MeasureModel measure);
    }
}
