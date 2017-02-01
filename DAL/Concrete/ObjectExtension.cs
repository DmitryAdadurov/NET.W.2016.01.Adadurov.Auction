using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
        {
            if (obj == null)
                return true;
            return false;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return true;
            else
                return false;
        }
    }
}
