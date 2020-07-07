using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subscriber.Data.Entities
{


    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //uniqe
        public string Email { get; set; }
        [Encrypted]
        public string Password { get; set; }

        public bool IsVerified { get; set; }
        public string ActivationCode { get; set; }
        public virtual UserFile UserFile { get; set; }
    }
}
