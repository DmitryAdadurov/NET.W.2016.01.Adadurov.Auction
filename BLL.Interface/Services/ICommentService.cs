using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface ICommentService : IService<BllComment, int>
    {
        Task<IEnumerable<BllComment>> GetRange(int skip, int take = 12, Expression<Func<BllComment, bool>> predicate = null);
        Task<int> PostComment(string text, string userName, int lotId);
        Task Delete(int commentId);
    }
}
