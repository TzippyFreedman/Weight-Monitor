using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackingService.Data.Entities;
using TrackingService.Services.Models;
using TrackingService.WebApi.DTO;

namespace TrackingService.WebApi
{
    public class UserRecordProfile : Profile
    {
        public UserRecordProfile()
        {
            CreateMap<Record, UserRecordModel>()
                .ReverseMap();
            CreateMap<UserRecordModel, UserRecordDTO>();
        }
    }
}
