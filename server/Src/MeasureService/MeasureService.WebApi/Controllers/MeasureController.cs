using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeasureService.Services;
using MeasureService.Services.Models;
using MeasureService.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeasureService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasureController : ControllerBase
    {


        private readonly ILogger<MeasureController> _logger;
        private readonly IMeasureService _measureService;
        private readonly IMapper _mapper;

        public MeasureController(IMeasureService measureService, IMapper mapper)
        {
            _measureService = measureService;
            _mapper = mapper;
        }

        [HttpPost]
        public async void Post(MeasureDTO measure)
        {
            MeasureModel measureModel = _mapper.Map<MeasureModel>(measure);
            measureModel.Status = Services.Enums.MeasureStatus.Pending;
            await _measureService.Add(measureModel);
        }
    }
}
