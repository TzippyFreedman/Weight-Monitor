using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
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

        public async Task<UserFileModel> GetUserFileById(Guid userFileId)
        {
            return await _userRepository.GetUserFileById(userFileId);
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

        public async Task<UserModel> RegisterAsync(UserModel user, UserFileModel userFile)
        {
            if (!_userRepository.CheckExists(user.Email))
            {
                UserModel userAdded = await _userRepository.AddUserAsync(user);
                if (userAdded != null)
                {
                    userFile.UserId = userAdded.Id;
                    UserFileModel userFileAdded = await _userRepository.AddUserFileAsync(userFile);
                    if (userFileAdded != null)
                    {
                        return userAdded;
                    }
                    else
                    {
                        return null;
                    }

                }
                return null;
            }
            else
            {
                return null;
            }



        }

        public async Task VerifyUserAsync(string emailAddress)
        {
            /*            if (!_userRepository.CheckExists(emailAddress))
                        {
                            ret
                        }*/

            Guid vertificationCode = Guid.NewGuid();
/*          await  _userRepository.AddVertificationCodeToUser(emailAddress, vertificationCode);
*/
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("rivkifreedman1@gmail.com"); //enter whatever email you are sending from here 
                mail.To.Add(emailAddress); //Text box that the user enters their email address 
                mail.Subject = "Email Vertification"; //enter whatever subject you would like 
                mail.Body = $"Your Activation Code is: {vertificationCode}"; 
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("rivkifreedman1@gmail.com", 587)) //enter the same email that the message is sending from along with port 587
                {
                    smtp.Host = "smtp.gmail.com";

                    smtp.Credentials = new NetworkCredential("rivkifreedman1@gmail.com", "er0533150865"); //Enter email with password 
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(mail);
                }


            }


        }
    }
}
