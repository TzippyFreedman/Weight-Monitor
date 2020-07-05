using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
    public interface IUserRepository
    {
        Task<UserModel> AddUserAsync(UserModel userRegister);

        int Login(LoginModel userRegister);
        bool CheckExists(UserModel userRegister);
        Task<UserFileModel> AddUserFileAsync(UserFileModel userFileSubscriber);
    }
}
