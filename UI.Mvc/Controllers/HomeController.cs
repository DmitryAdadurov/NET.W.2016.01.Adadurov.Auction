using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Mvc.Infrastructure.Mappers;
using UI.Mvc.Models;

namespace UI.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuctionService<int> auctionService;
        private readonly ICategoryAccessor categoryAccessor;
        public HomeController(IAuctionService<int> ias, ICategoryAccessor ica)
        {
            auctionService = ias;
            categoryAccessor = ica;
        }

        public async Task<ActionResult> Index()
        {
            var lots = (await auctionService.GetRange(0)).Select(t => t.ToLotMvc());
            IEnumerable<Category> categories = (await categoryAccessor.GetAll()).Select(t => t.ToMvcCategory());
            ViewData["Categories"] = categories;
            return View(lots);
        }
    }
}