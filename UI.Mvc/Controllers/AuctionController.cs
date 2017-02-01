using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Mvc.Infrastructure.Mappers;
using UI.Mvc.Models;

namespace UI.Mvc.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly IAuctionService<int> auctionService;
        private readonly IUserService userService;
        private readonly IBidService bidService;
        private readonly ICategoryAccessor categoriesAccessor;
        public AuctionController(IAuctionService<int> ias, IUserService ius, IBidService ibs, ICategoryAccessor ics)
        {
            auctionService = ias;
            userService = ius;
            bidService = ibs;
            categoriesAccessor = ics;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> My(int? page)
        {
            int pageSize = 20;
            page = page ?? 1;
            int userId = await userService.GetId(User.Identity.Name);
            var results = (await auctionService.GetRange((page.Value - 1) * pageSize, pageSize, t => t.Seller == userId)).Select(t => t.ToLotMvc());

            if (results == null)
                ViewData["NoResults"] = "No auctions";

            var searchResult = new SearchResult<LotModel>(results, auctionService.Count(t => t.Seller == userId) / pageSize, page.Value);

            return View(searchResult);
        }

        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> MyBids(int? page)
        {
            int pageSize = 20;
            page = page ?? 1;
            int userId = await userService.GetId(User.Identity.Name);
            var results = (await bidService.GetRange((page.Value - 1) * pageSize, pageSize, t => t.User == userId)).Select(t => t.ToMvcBid(userService));

            if (results == null)
                ViewData["NoResults"] = "No auctions";

            var searchResult = new SearchResult<Bid>(results, bidService.Count(t => t.User == userId) / pageSize, page.Value);

            return View(searchResult);
        }

        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> Create()
        {
            IEnumerable<SelectListItem> cats = (await categoriesAccessor.GetAll()).Select(t => new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
            ViewData["Categories"] = cats;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> Create(AddAuctionModel auctionModel)
        {
            if (ModelState.IsValid)
            {
                var folder = Guid.NewGuid();
                string photos = null;
                foreach (var file in auctionModel.Photos)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var filename = Guid.NewGuid();
                        string ext = Path.GetExtension(file.FileName);
                        string path = $"/upload/{folder}/";
                        Directory.CreateDirectory(Server.MapPath(path));
                        path = $"{path}{filename}{ext}";
                        photos = $"{photos}:{path}";
                        file.SaveAs(Server.MapPath(path));
                    }
                }
                photos = photos?.Remove(0, 1);

                BllLot lot = new BllLot()
                {
                    AuctionEndDate = auctionModel.AuctionEndDate,
                    Categorie = int.Parse(auctionModel.Categorie),
                    CreationDate = DateTime.Now,
                    CurrentPrice = auctionModel.StartPrice,
                    Description = auctionModel.Description,
                    LastUpdateDate = DateTime.Now,
                    Photos = photos,
                    Seller = await userService.GetId(User.Identity.Name),
                    Title = auctionModel.Title
                };

                int auctionId = await auctionService.Create(lot);

                return RedirectToAction("Details", new { id = auctionId });
            }
            return View(auctionModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id.Value < 1)
                return RedirectToAction("Index", "Home");

            var auction = await auctionService.GetTotalAuctionInfo(id.Value, 0);
            if (auction == null)
                return RedirectToAction("Index", "Home");
            await auctionService.Delete(auction);            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || id < 1)
                return RedirectToAction("Index", "Home");

            var bllAuction = await auctionService.GetTotalAuctionInfo((int)id, 10);

            if (bllAuction == null)
                return RedirectToAction("Index", "Home");

            if (TempData["ViewData"] != null)
                ViewData = (ViewDataDictionary)TempData["ViewData"];

            AuctionDetail mvcAuction = bllAuction.ToMvcAuctionDetail(userService);

            return View(mvcAuction);
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> Bet(int? auctionId, decimal betValue)
        {
            if (auctionId == null || auctionId < 1)
                return RedirectToAction("Index", "Home");

            if (betValue <= 0)
            {
                ModelState.AddModelError("betValue", "Bet value can't be negative.");
            }

            if (ModelState.IsValid)
            {
                if (await bidService.PlaceBet((int)auctionId, User.Identity.Name, betValue))
                {
                    return RedirectToAction("Details", new { id = auctionId });
                }
                else
                {
                    return RedirectToAction("Details", new { id = auctionId });
                }
            }

            TempData["ViewData"] = ViewData;
            return RedirectToAction("Details", new { id = auctionId });
        }

        [Authorize(Roles = "user, admin")]
        public ActionResult FilesTooLarge()
        {
            return View();
        }
    }
}