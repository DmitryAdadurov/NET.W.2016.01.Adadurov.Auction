using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UI.Mvc.Models;

namespace UI.Mvc.Infrastructure.Mappers
{
    public static class CommentMapper
    {
        public static Comment ToMvcComment(this BllComment comment, IUserService userService)
        {
            Task<BllUser> task = Task.Run(() => userService.GetById(comment.User));
            task.Wait();

            return new Comment()
            {
                Id = comment.Id,
                UserId = comment.User,
                PubDate = comment.Date,
                Text = comment.Text,
                UserName = task.Result.Login,
                LotId = comment.Lot
            };
        }
    }
}