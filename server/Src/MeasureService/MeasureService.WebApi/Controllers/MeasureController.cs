using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeasureService.Services;
using MeasureService.Services.Models;
using MeasureService.WebApi.DTO;
using Messages.Commands;
using Messages.Enums.MeasureStatus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace MeasureService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasureController : ControllerBase
    {


        private readonly ILogger<MeasureController> _logger;
        private readonly IMeasureService _measureService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public MeasureController(IMeasureService measureService, IMapper mapper, IMessageSession messageSession)
        {
            _measureService = measureService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost]
        public async Task Post(MeasureDTO measure)
        {
            MeasureModel measureModel = _mapper.Map<MeasureModel>(measure);
            measureModel.Status = MeasureStatus.Pending;
            MeasureModel newMeasureModel =  await _measureService.Add(measureModel);

            await _messageSession.Send<IUpdateUserFile>(message =>
            {
                message.MeasureId = newMeasureModel.Id;
                message.Weight = measure.Weight;
                message.UserFileId = measure.UserFileId;
            });


        }
    }
}
