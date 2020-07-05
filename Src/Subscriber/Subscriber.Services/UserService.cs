using AutoMapper;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _pathRepository;

        public UserService(IUserRepository pathRepository)
        {
            _pathRepository = pathRepository;
        }
        public int Login(LoginModel userRegister)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> RegisterAsync(UserModel user,UserFileModel userFile)
        {
          if(!  _pathRepository.CheckExists(user))
            {
                UserModel userAdded = await _pathRepository.AddUserAsync(user);
                if (userAdded != null)
                {
                    userFile.SubscriberId = userAdded.Id;
                    UserFileModel userFileAdded= await  _pathRepository.AddUserFileAsync(userFile);
                    if(userFileAdded!=null)
                    {
                        return userAdded;
                    }
                    else
                    {
                        return null;
                    }

                }
                return null;
            }
            else
            {
                return null;
            }


           
        }

       
    }
}
