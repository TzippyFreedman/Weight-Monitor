using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
  public  interface IUserService
    {
         Task<UserModel> RegisterAsync(UserModel userRegister,UserFileModel userFileModel);

         int Login(LoginModel userRegister);


    }
}
