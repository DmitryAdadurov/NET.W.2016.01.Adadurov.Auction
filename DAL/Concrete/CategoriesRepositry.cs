using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.Entities;
using System.Linq.Expressions;
using System.Data.Entity;
using ORM.EF;
using DAL.Concrete.Mappers;
using DAL.Interface;

namespace DAL.Concrete
{
    public class CategoriesRepositry : GenericRepository<Category, DalCategory>, ICategoriesRepository
    {
        public CategoriesRepositry(DbContext dbContext) : base(dbContext, new CategoryDalCategoryMapper())
        {
        }
        
    }
}
