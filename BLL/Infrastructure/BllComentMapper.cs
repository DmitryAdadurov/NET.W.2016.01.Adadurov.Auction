using BLL.Interface.Entities;
using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public static class BllComentMapper
    {
        public static BllComment ToBllComment(this DalCommentary comment)
        {
            return new BllComment()
            {
                Id = comment.Id,
                Date = comment.Date,
                Text = comment.Text,
                Lot = comment.Lot,
                User = comment.User
            };
        }

        public static DalCommentary ToDalCommentary(this BllComment comment)
        {
            return new DalCommentary()
            {
                Id = comment.Id,
                Date = comment.Date,
                Text = comment.Text,
                Lot = comment.Lot,
                User = comment.User
            };
        }
    }
}
