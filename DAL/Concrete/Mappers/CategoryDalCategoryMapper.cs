using DAL.Interface;
using DAL.Interface.Entities;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Mappers
{
    public class CategoryDalCategoryMapper : IMapper<Category, DalCategory>
    {
        //public static DalCategory ToDalCategory(this Category cat)
        //{
        //    return new DalCategory()
        //    {
        //        Id = cat.Id,
        //        Name = cat.Name
        //    };
        //}

        //public static Category ToEFCategory(this DalCategory dalCat)
        //{
        //    return new Category()
        //    {
        //        Id = dalCat.Id,
        //        Name = dalCat.Name
        //    };
        //}
        public DalCategory ToDalEntity(Category entity)
        {
            return new DalCategory()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Category ToEFEntity(DalCategory dalEntity)
        {
            return new Category()
            {
                Id = dalEntity.Id,
                Name = dalEntity.Name
            };
        }
    }
}
