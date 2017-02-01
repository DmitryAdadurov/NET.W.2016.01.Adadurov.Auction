using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface ILotRepository : IRepository<DalLot>
    {
        Task<DalLot> GetById(int key);
    }
}
