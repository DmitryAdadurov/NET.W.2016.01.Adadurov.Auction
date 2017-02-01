using BLL.Infrastructure;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CategoriesAccessor : ICategoryAccessor
    {
        private readonly IUnitOfWork context;
        public CategoriesAccessor(IUnitOfWork uow)
        {
            context = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<int> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return await context.CategoriesRepository.Create(new DAL.Interface.Entities.DalCategory() { Name = name });
        }

        public async Task Delete(int id)
        {
            await context.CategoriesRepository.Delete(new DAL.Interface.Entities.DalCategory() { Id = id });
        }

        public async Task<IEnumerable<BllCategorie>> GetAll()
        {
            return (await context.CategoriesRepository.GetAll()).Select(t => t.ToBllCategorie());
        }

        public async Task<BllCategorie> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return (await context.CategoriesRepository.GetByPredicate(t => t.Name == name)).FirstOrDefault().ToBllCategorie();
        }

        public async Task<BllCategorie> GetById(int id)
        {
            return (await context.CategoriesRepository.GetByPredicate(t => t.Id == id)).FirstOrDefault().ToBllCategorie();
        }
    }
}
