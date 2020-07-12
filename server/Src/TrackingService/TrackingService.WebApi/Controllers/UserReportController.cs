using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackingService.Services;
using TrackingService.Services.Models;
using TrackingService.WebApi.DTO;

namespace TrackingService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReportController : ControllerBase
    {
        private readonly ITrackingService _trackingService;
        private readonly IMapper _mapper;

        public UserReportController(ITrackingService trackingService, IMapper mapper)
        {
            _trackingService = trackingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<UserRecordDTO>> Get(Guid UserFileId)
        {
            List<UserRecordModel> records = await _trackingService.GetRecordsByUserFileId(UserFileId);
            return _mapper.Map<List<UserRecordDTO>>(records);
        }
    }
}