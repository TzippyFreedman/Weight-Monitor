using AutoMapper;
using MeasureService.Data.Entities;
using MeasureService.Services;
using MeasureService.Services.Models;
using System;
using System.Collections.Generic;
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
        public async Task Add(MeasureModel measureModel)
        {
            Measure measure = _mapper.Map<Measure>(measureModel);
            _measureDbContext.Measures.Add(measure);
          await  _measureDbContext.SaveChangesAsync();
        }
    }
}
