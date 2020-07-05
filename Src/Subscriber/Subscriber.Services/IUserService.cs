using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services
{
    interface IUserService
    {
        bool register(Register userRegister);

        int login(Register userRegister);


    }
}
