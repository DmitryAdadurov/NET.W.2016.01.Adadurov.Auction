using BLL.Interface.Entities;
using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public static class BllCategorieMapper
    {
        public static BllCategorie ToBllCategorie(this DalCategory cat)
        {
            return new BllCategorie()
            {
                Id = cat.Id,
                Name = cat.Name
            };
        }

        public static DalCategory ToDalCategory(this BllCategorie cat)
        {
            return new DalCategory()
            {
                Id = cat.Id,
                Name = cat.Name
            };
        }
    }
}
