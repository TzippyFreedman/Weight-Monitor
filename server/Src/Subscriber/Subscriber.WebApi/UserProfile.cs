using AutoMapper;
using Subscriber.Data.Entities;
using Subscriber.Services.Models;
using Subscriber.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.WebApi
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, User>()
                .ReverseMap();
           CreateMap<UserFileModel, UserFile>()
                .ReverseMap();
           CreateMap<UserFileModel, UserFileDTO>();

            CreateMap<SubscriberDTO, UserModel>()
                .ReverseMap()
                 .ForMember(dest => dest.Height, opt => opt.Ignore());

            var map = CreateMap<SubscriberDTO, UserFileModel>();
            map.ForAllMembers(opt => opt.Ignore());
            map.ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height));

            CreateMap<UserFileModel, UserFile>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
            CreateMap<UserModel, User>()
                .ReverseMap();
        }
    }
}
