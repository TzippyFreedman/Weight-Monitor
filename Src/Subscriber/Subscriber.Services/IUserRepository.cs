using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
   public interface IUserRepository
    {
/*        bool Register(RegisterModel userRegister);
*/
        Task<UserModel> LoginAsync(string email, string password);
        Task<Guid> GetUserFileIdByUserId(Guid id);
        Task<UserFileModel> GetUserFileById(Guid userCardId);
    }
}
