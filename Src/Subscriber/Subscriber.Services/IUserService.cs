using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Services
{
  public interface IUserService
    {

        Task<Guid> LoginAsync(string Email, string Password);
        Task<UserFileModel> GetUserFileById(Guid userCardId);
    }
}
