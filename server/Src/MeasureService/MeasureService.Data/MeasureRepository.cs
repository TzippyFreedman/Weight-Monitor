using AutoMapper;
using MeasureService.Data.Entities;
using MeasureService.Data.Exceptions;
using MeasureService.Services;
using MeasureService.Services.Models;
using Messages.Enums.MeasureStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Data
{
    public class MeasureRepository : IMeasureRepository
    {
        private readonly MeasureDbContext _measureDbContext;
        private readonly IMapper _mapper;

        public MeasureRepository(MeasureDbContext measureDbContext, IMapper mapper)
        {
            _measureDbContext = measureDbContext;
            _mapper = mapper;
        }
        public async Task<MeasureModel> Add(MeasureModel measureModel)
        {
            Measure measure = _mapper.Map<Measure>(measureModel);
            _measureDbContext.Measures.Add(measure);
            await _measureDbContext.SaveChangesAsync();
            return _mapper.Map<MeasureModel>(measure);
        }

        public async Task UpdateStatusAsync(Guid measureId, MeasureStatus status, string comments)
        {
            Measure measureToUpdate;
            try
            {
                 measureToUpdate = _measureDbContext.Measures.Where(t => t.Id == measureId).FirstOrDefault();

            }
            catch(Exception e)
            {
                measureToUpdate = new Measure();
            }

            if (measureToUpdate == null)
            {
                throw new MeasureNotFoundExeption();
            }

            measureToUpdate.Status = status;
            measureToUpdate.Comments = comments;
            //await _measureDbContext.SaveChangesAsync();

        }


    }
}
