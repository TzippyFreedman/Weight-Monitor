using AutoMapper;
using Subscriber.Data.Entities;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data
{
    class UserProfile:Profile
    {
        public UserProfile()
        {
            this.CreateMap<UserFile, UserFileModel>();
            this.CreateMap<UserModel, User>();
            //this.CreateMap<RegisterModel,UserModel>()
            //    .ReverseMap()
            //.ForMember(dest => dest.Height, opt => opt.Ignore());
        }
    }
}
