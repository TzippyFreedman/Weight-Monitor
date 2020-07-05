using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services
{
    interface IUserService
    {
        bool register(UserRegister userRegister);

        int login(UserRegister userRegister);


    }
}
