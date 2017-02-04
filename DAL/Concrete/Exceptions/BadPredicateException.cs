using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Exceptions
{
    /// <summary>
    /// Throws if conversion of predicate is failed
    /// </summary>
    public class BadPredicateException : Exception
    {
        public BadPredicateException()
        {
        }

        public BadPredicateException(string message) : base(message)
        {
        }

        public BadPredicateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
