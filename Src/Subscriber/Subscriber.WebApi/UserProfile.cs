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
            this.CreateMap<UserModel, User>()
                .ReverseMap();
            this.CreateMap<UserFileModel, UserFile>()
                .ReverseMap();
            this.CreateMap<UserFileModel, UserFileDTO>();
        }
    }
}
