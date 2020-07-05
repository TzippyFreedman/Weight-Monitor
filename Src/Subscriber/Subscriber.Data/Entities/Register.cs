using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data.Entities
{
   public class Register
    {
        public Guid Id { get; private set; }

        public string FirstName { get; set; }
        public int LastName { get; set; }
        //uniqe
        public string Email { get; set; }
        public string Password { get; set; }
        public int Height { get; set; }
    }
}
