using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data.Entities
{
  public  class UserFile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OpenDate { get; set; }
        public float BMI { get; set; }

        public float Height { get; set; }
        public float Weight { get; set; }

        public DateTime UpdateDate { get; set; }
        public virtual User User { get; set; }


    }
}
