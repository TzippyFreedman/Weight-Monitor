using AutoMapper;
using MeasureService.Data.Entities;
using MeasureService.Services.Models;
using MeasureService.WebApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasureService.WebApi
{
    public class MeasureProfile : Profile
    {
        public MeasureProfile()
        {
            CreateMap<MeasureDTO, MeasureModel>();
            CreateMap<MeasureModel, Measure>()
                .ReverseMap();

        }
       
    }
}
