using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public enum UserVerificationResult
    {
        NotExist = 1, ExistWithSameLogin = 2, ExistWithSameEmail = 3
    }
}
