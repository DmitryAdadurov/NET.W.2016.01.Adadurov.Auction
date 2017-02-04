using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Exceptions
{
    /// <summary>
    /// Throws if Set for provided entity is not exist
    /// </summary>
    public class WrongEntityException : Exception
    {
        public WrongEntityException()
        {
        }

        public WrongEntityException(string message) : base(message)
        {
        }

        public WrongEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
