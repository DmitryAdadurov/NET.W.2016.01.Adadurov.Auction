using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Exceptions
{
    public class DatabaseErrorException : Exception
    {
        /// <summary>
        /// Throws if database not exists
        /// </summary>
        public DatabaseErrorException()
        {
        }

        public DatabaseErrorException(string message) : base(message)
        {
        }

        public DatabaseErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
