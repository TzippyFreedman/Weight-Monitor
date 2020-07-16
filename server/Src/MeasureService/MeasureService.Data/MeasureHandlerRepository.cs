using MeasureService.Data.Entities;
using MeasureService.Data.Exceptions;
using MeasureService.Handlers;
using Messages.Enums.MeasureStatus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Data
{
    public class MeasureHandlerRepository : IMeasureHandlerRepository
    {
        private readonly MeasureDbContext _measureDbContext;

        public MeasureHandlerRepository(MeasureDbContext measureDbContext)
        {
            _measureDbContext = measureDbContext;
        }
        public async Task UpdateStatusAsync(Guid measureId, MeasureStatus status, string comments)
        {
            Measure measureToUpdate;
            
            measureToUpdate = await _measureDbContext.Measures
                .Where(t => t.Id == measureId)
                .FirstOrDefaultAsync();

            if (measureToUpdate == null)
            {
                throw new MeasureNotFoundExeption();
            }

            measureToUpdate.Status = status;
            measureToUpdate.Comments = comments;
        }
    }
}
