using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserFileModel> GetUserFileById(Guid userCardId)
        {
            return await _userRepository.GetUserFileById(userCardId);
        }

        public async Task<Guid> LoginAsync(string email, string password)
        {
            UserModel user = await _userRepository.LoginAsync(email, password);

            if (user == null)
            {
                return Guid.Empty;
            }
            else
            {
                Guid userFileId = await _userRepository.GetUserFileIdByUserId(user.Id);
                return userFileId;
            }

        }


    }
}
