using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Mvc.Models;

namespace UI.Mvc.Infrastructure.Mappers
{
    public static class CategoryMapper
    {
        public static Category ToMvcCategory(this BllCategorie cat)
        {
            return new Category()
            {
                Id = cat.Id,
                Name = cat.Name
            };
        }

        public static BllCategorie ToBllCategory(this Category cat)
        {
            return new BllCategorie()
            {
                Id = cat.Id,
                Name = cat.Name
            };
        }
    }
}