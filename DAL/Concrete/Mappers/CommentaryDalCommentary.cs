using DAL.Interface;
using DAL.Interface.Entities;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Mappers
{
    public class CommentaryDalCommentary : IMapper<Commentary, DalCommentary>
    {
        //public static DalCommentary ToDalCommentary(this Commentary comment)
        //{
        //    return new DalCommentary()
        //    {
        //        Id = comment.Id,
        //        Date = comment.Date,
        //        User = comment.User.ToDalUser(),
        //        Lot = comment.Lot.ToDalLot(),
        //        Text = comment.Text
        //    };
        //}

        //public static Commentary ToEFCommentary(this DalCommentary dalComment)
        //{
        //    return new Commentary()
        //    {
        //        Id = dalComment.Id,
        //        Date = dalComment.Date,
        //        User = dalComment.User.ToUserEntity(),
        //        Lot = dalComment.Lot.ToEFLot(),
        //        Text = dalComment.Text
        //    };
        //}

        //public static IEnumerable<DalCommentary> ToDalComments(this ICollection<Commentary> comments)
        //{
        //    return comments.Select(t => t.ToDalCommentary());
        //}

        //public static ICollection<Commentary> ToComments(this IEnumerable<DalCommentary> dalComments)
        //{
        //    return dalComments.Select(t => t.ToEFCommentary()).ToList();
        //}
        public DalCommentary ToDalEntity(Commentary entity)
        {
            return new DalCommentary()
            {
                Id = entity.Id,
                Date = entity.Date,
                User = entity.UserId,
                Lot = entity.LotId,
                Text = entity.Text
            };
        }

        public Commentary ToEFEntity(DalCommentary dalEntity)
        {
            return new Commentary()
            {
                Id = dalEntity.Id,
                Date = dalEntity.Date,
                UserId = dalEntity.User,
                LotId = dalEntity.Lot,
                Text = dalEntity.Text
            };
        }
    }
}
