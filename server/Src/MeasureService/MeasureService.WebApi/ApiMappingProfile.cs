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
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<MeasureDTO, MeasureModel>();
           /* CreateMap<MeasureModel, Measure>()
                .ReverseMap();*/

        }
       
    }
}
