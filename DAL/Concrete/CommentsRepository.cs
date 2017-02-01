using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.Entities;
using System.Linq.Expressions;
using ORM.EF;
using DAL.Concrete.Mappers;

namespace DAL.Concrete
{
    public class CommentsRepository : GenericRepository<Commentary, DalCommentary>, ICommentsRepository
    {
        public CommentsRepository(DbContext dbContext) : base(dbContext, new CommentaryDalCommentary())
        {
        }
        
        public async Task DeleteRange(IEnumerable<DalCommentary> commentaries)
        {
            IEnumerable<int> ids = commentaries.Select(t => t.Id);
            IEnumerable<Commentary> comments = context.Set<Commentary>().Where(t => ids.Contains(t.Id));
            context.Set<Commentary>().RemoveRange(comments);
        }

        public async Task<IEnumerable<DalCommentary>> FindAllLotComments(int lotId)
        {
            return (await context.Set<Commentary>().Where(t => t.LotId == lotId).OrderBy(t => t.Date).ToListAsync()).ConvertAll(converter.ToDalEntity);
        }

        public DalCommentary GetById(int key)
        {
            throw new NotImplementedException();
        }
    }
}
