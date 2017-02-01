using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface ICommentsRepository : IRepository<DalCommentary>
    {
        Task DeleteRange(IEnumerable<DalCommentary> commentaries);
        Task<IEnumerable<DalCommentary>> FindAllLotComments(int lotId);
    }
}
