using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
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
   public class UserRepository: IUserRepository
    {
        private readonly UserContext _userDbContext;
        private readonly IMapper _mapper;

        public UserRepository(UserContext userDbContext, IMapper mapper)
        {
            _userDbContext = userDbContext;
            _mapper = mapper;
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            User userToAdd = _mapper.Map<User>(user);
             _userDbContext.Users.Add(userToAdd);
            await _userDbContext.SaveChangesAsync();
             return _mapper.Map<UserModel>(userToAdd);
        }
        public async Task<UserFileModel> AddUserFileAsync(UserFileModel userFileSubscriber)
        {
            UserFile userToAdd = _mapper.Map<UserFile>(userFileSubscriber);
             _userDbContext.UserFiles.Add(userToAdd);
            await _userDbContext.SaveChangesAsync(); 
            //return _mapper.Map<UserModel>(userToAdd);
            return _mapper.Map<UserFileModel>(userToAdd);
            
        }

       

        public bool CheckExists(UserModel userRegister)
        {
          return  _userDbContext.Users.Any(u => u.Email == userRegister.Email);
        }

        public int Login(LoginModel userRegister)
        {
            throw new NotImplementedException();
        }

  

      
    }

   
}
