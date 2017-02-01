using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Mvc.Infrastructure.Mappers;

namespace UI.Mvc.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IUserService userService;
        public CommentController(ICommentService ics, IUserService ius)
        {
            commentService = ics;
            userService = ius;
        }

        [Authorize]
        public async Task<ActionResult> Add(string text, int? lotid)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            if (Request.IsAjaxRequest())
            {
                int comentId = await commentService.PostComment(text, User.Identity.Name, lotid.Value);
                var commentView = (await commentService.GetById(comentId)).ToMvcComment(userService);

                return PartialView("Partial/Comment", commentView);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> Delete(int? id, int? auctionId)
        {
            if (id == null || auctionId == null || id.Value < 1 || auctionId.Value < 1)
                return RedirectToAction("Index", "Home");

            await commentService.Delete(id.Value);
            return RedirectToAction("Details", "Auction", new { id = auctionId });
        }

        public async Task<ActionResult> GetComments(int? page, int? lotid)
        {
            int pageSize = 10;
            if (Request.IsAjaxRequest())
            {
                page = page ?? 1;
                var results = (await commentService.GetRange((page.Value - 1) * pageSize, pageSize, t => t.Lot == lotid)).Select(t => t.ToMvcComment(userService));
                return PartialView("Partial/CommentsPartial", results);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}