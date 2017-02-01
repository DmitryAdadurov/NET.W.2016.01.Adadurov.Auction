using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using ORM.EF;
using DAL.Concrete.Mappers;
using DAL.Interface.Repository;
using DAL.Interface.Entities;

namespace DAL.Concrete
{
    public class LotRepository : GenericRepository<Lot, DalLot>, ILotRepository
    {
        public LotRepository(DbContext dbContext) : base(dbContext, new LotDalLotMappers())
        {
        }
        public async Task<DalLot> GetById(int key)
        {
            return converter.ToDalEntity(await context.Set<Lot>().FirstOrDefaultAsync(t => t.Id == key));
        }
    }
}
