using Messages.Enums.MeasureStatus;
using System;
using System.Threading.Tasks;

namespace MeasureService.Handlers.Services
{
    public interface IMeasureHandlersRepository
    {
        Task UpdateStatusAsync(Guid measureId, MeasureStatus status, string comments);
    }
}
