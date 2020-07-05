using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services.Models
{
   public class UserFileModel
    {
        public Guid Id { get; set; }
        public Guid SubscriberId { get; set; }
        public DateTime OpenDate { get; set; }
        public float BMI { get; set; }

        public float Height { get; set; }
        public float Weight { get; set; }

        public DateTime UpdateDate { get; set; }

      


    }
}
