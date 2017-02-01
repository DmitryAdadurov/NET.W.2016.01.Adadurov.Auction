using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface ICategoryAccessor
    {
        Task<int> Create(string name);
        Task Delete(int id);
        Task<IEnumerable<BllCategorie>> GetAll();
        Task<BllCategorie> GetByName(string name);
        Task<BllCategorie> GetById(int id);
    }
}
