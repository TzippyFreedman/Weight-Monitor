using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services
{
    interface IUserRepository
    {
        bool register(Register userRegister);

        int login(Login userRegister);
    }
}
