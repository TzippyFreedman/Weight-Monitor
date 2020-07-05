using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.WebApi.Models
{
    public class UserFileDTO
    {
        //public int Id { get; set; }
        public Guid SubscriberId { get; set; }
        public DateTime OpenDate { get; set; }
        public float BMI { get; set; }

        public float Height { get; set; }
        public float Weight { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
