using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscriber.Data.Entities;
using Subscriber.Services;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace Subscriber.Data
{
   public class UserRepository : IUserRepository
    {

        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;
        public UserRepository(UserDbContext userDbContext, IMapper mapper)
        {
            _userDbContext = userDbContext;
            _mapper = mapper;

        }

        public async Task<UserFileModel> GetUserFileById(Guid userCardId)
        {

            UserFile file = await _userDbContext.UserFiles
                            .Where(file => file.Id == userCardId)
                            .FirstOrDefaultAsync();
            if (file == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<UserFileModel>(file);

            }

        }



        public async Task<Guid> GetUserFileIdByUserId(Guid id)
        {

            UserFile file = await _userDbContext.UserFiles
                .Where(file => file.User.Id == id)
                .FirstOrDefaultAsync();

            if (file != null)
            {
                return file.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public async Task<UserModel> LoginAsync(string email, string password)
        {



                
        User user = await _userDbContext.Users.Where(user =>
                       user.Email == email && user.Password == password)
                        .FirstOrDefaultAsync();

            if (user != null)
            {
                return _mapper.Map<UserModel>(user);
            }
            else
            {
                return null;
            }
            
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            User userToAdd = _mapper.Map<User>(user);
            _userDbContext.Users.Add(userToAdd);
            await _userDbContext.SaveChangesAsync();
            return _mapper.Map<UserModel>(userToAdd);
        }
        public async Task<UserFileModel> AddUserFileAsync(UserFileModel userFileSubscriber)
        {
            UserFile userToAdd = _mapper.Map<UserFile>(userFileSubscriber);
            _userDbContext.UserFiles.Add(userToAdd);
            await _userDbContext.SaveChangesAsync();
            return _mapper.Map<UserFileModel>(userToAdd);

        }



        public bool CheckExists(string emailAddress)
        {
            return _userDbContext.Users.Any(u => u.Email == emailAddress);
        }

        public async Task AddVertificationCodeToUser(string emailAddress, Guid activationCode)
        {
/*            User user = _userDbContext.Users
                    .Where(user => user.Email == emailAddress)
                    .FirstOrDefault();
            user.ActivationCode = activationCode;

          await  _userDbContext.SaveChangesAsync();*/
        }
    }
}
