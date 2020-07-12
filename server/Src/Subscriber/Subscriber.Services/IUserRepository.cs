using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
   public interface IUserRepository
    {
        bool CheckExists(string emailAddress);
        Task<UserFileModel> AddUserFileAsync(UserFileModel userFileSubscriber);
        Task<UserModel> AddUserAsync(UserModel userRegister);

        Task<UserModel> LoginAsync(string email, string password);
        Task<Guid> GetUserFileIdByUserId(Guid id);
        Task<UserFileModel> GetUserFileById(Guid userCardId);
        Task<bool> CheckUserFileExists(Guid userfileId);
        Task AddVertificationCodeToUser(string emailAddress, Guid vertificationCode);
        Task<float> UpdateWeight(Guid userfileId, float weight);
    }
}
