using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services
{
  public  interface IUserService
    {
         bool register(RegisterModel userRegister);

         int login(LoginModel userRegister);


    }
}
