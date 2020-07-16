using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Handlers
{
    public interface IMeasureHandlerRepository
    {
        Task UpdateStatusAsync(Guid measureId,MeasureStatus status, string comments);

    }
}
