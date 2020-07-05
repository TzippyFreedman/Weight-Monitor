using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services
{
    interface IUserService
    {
        bool register(RegisterModel userRegister);

        int login(RegisterModel userRegister);


    }
}
