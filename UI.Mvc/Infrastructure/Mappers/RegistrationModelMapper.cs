using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Mvc.Models;

namespace UI.Mvc.Infrastructure.Mappers
{
    public static class RegistrationModelMapper
    {
        public static BllUser ToBllUser(this RegisterModel reg)
        {
            return new BllUser()
            {
                Login = reg.UserName,
                Email = reg.Email,
                FullName = reg.FullName,
                Country = reg.Country
            };
        }
    }
}