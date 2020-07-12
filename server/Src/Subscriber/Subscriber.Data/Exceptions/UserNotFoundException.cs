using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {

        }
        public UserNotFoundException(Guid userId) : base($"User with id:{userId} does not exist")
        {

        }
    }
}
