using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subscriber.Services.Models
{
    public class UserModel
    {
        public Guid Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //unique
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual UserFileModel UserFile { get; set; }

    }
}
