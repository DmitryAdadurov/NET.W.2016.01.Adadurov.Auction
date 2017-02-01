using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class NotExistingUserException : Exception
    {
        public NotExistingUserException()
        {
        }

        public NotExistingUserException(string message) : base(message)
        {
        }

        public NotExistingUserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
