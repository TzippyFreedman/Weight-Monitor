﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscriber.Data.Entities;
using Subscriber.Services;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Data
{
   public class UserRepository : IUserRepository
    {

        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;
        public UserRepository(UserDbContext userDbContext, IMapper mapper)
        {
            _userDbContext = userDbContext;
            _mapper = mapper;

        }

        public async Task<UserFileModel> GetUserFileById(Guid userCardId)
        {
            var x = new User { Email = "hhh", Password = "esther45" };
            _userDbContext.Add(x);
            _userDbContext.SaveChanges();
            UserFile file = await _userDbContext.UserFiles
                            .Where(file => file.Id == userCardId)
                            .FirstOrDefaultAsync();
            if (file == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<UserFileModel>(file);

            }

        }



        public async Task<Guid> GetUserFileIdByUserId(Guid id)
        {

            UserFile file = await _userDbContext.UserFiles
                .Where(file => file.User.Id == id)
                .FirstOrDefaultAsync();

            if (file != null)
            {
                return file.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public async Task<UserModel> LoginAsync(string email, string password)
        {
            User user = await _userDbContext.Users.Where(user =>
                       user.Email == email && user.Password == password)
                .Include(user=> user.UserFile)
                        .FirstOrDefaultAsync();

            if (user != null)
            {
                return _mapper.Map<UserModel>(user);
            }
            else
            {
                return null;
            }
            
        }

/*        public bool Register(RegisterModel userRegister)
        {
            throw new NotImplementedException();
        }*/
    }
}
