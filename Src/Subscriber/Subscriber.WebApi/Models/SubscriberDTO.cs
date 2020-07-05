using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.WebApi.Models
{
    public class SubscriberDTO
    {
        //public Guid Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //uniqe
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public int Height { get; set; }



    }
}
