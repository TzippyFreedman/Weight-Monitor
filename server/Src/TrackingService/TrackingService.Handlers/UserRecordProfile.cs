using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TrackingService.Data.Entities;
using TrackingService.Services.Models;

namespace TrackingService.Handlers
{
    class UserRecordProfile : Profile
    {
        public UserRecordProfile()
        {
            CreateMap<Record, UserRecordModel>()
                .ReverseMap();
        }
    }
}
