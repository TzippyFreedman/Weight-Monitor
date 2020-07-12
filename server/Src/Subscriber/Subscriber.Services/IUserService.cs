using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
  public interface IUserService
    {
        Task<UserModel> RegisterAsync(UserModel userRegister, UserFileModel userFileModel);
        Task<Guid> LoginAsync(string Email, string Password);
        Task<UserFileModel> GetUserFileById(Guid userFileId);
        Task VerifyUserAsync(string emailAddress);
        Task<bool> CheckUserFileExist(Guid userFileId);
        Task<float> UpdateWeight(Guid userFileId, float weight);
    }
}
