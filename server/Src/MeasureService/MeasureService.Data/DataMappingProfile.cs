using AutoMapper;
using MeasureService.Data.Entities;
using MeasureService.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureService.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<MeasureModel, Measure>()
                .ReverseMap();

        }
    }
}
