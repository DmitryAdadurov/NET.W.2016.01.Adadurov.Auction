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

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="name">Category name</param>
        /// <returns>Id of created category</returns>
        public async Task<int> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return await context.CategoriesRepository.Create(new DAL.Interface.Entities.DalCategory() { Name = name });
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id">Id of the category</param>
        public async Task Delete(int id)
        {
            await context.CategoriesRepository.Delete(new DAL.Interface.Entities.DalCategory() { Id = id });
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>Enumeration with categories</returns>
        public async Task<IEnumerable<BllCategorie>> GetAll()
        {
            return (await context.CategoriesRepository.GetAll()).Select(t => t.ToBllCategorie());
        }

        /// <summary>
        /// Get category by name
        /// </summary>
        /// <param name="name">Name of category</param>
        /// <returns>BllCategorie if succeded</returns>
        public async Task<BllCategorie> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return (await context.CategoriesRepository.GetByPredicate(t => t.Name == name)).FirstOrDefault().ToBllCategorie();
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id">Id of category</param>
        /// <returns>BllCategorie if succeded</returns>
        public async Task<BllCategorie> GetById(int id)
        {
            return (await context.CategoriesRepository.GetByPredicate(t => t.Id == id)).FirstOrDefault().ToBllCategorie();
        }
    }
}
