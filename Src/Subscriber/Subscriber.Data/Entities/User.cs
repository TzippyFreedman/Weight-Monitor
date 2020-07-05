using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data.Entities
{
   public class User
    {
        public Guid Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //uniqe
        
        public string Email { get; set; }

        public string Password { get; set; }
        //public int Height { get; set; }
        public virtual UserFile UserFile { get; set; }

    }
}
