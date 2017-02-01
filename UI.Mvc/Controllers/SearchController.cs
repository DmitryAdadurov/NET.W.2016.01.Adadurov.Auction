using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Mvc.Infrastructure.Mappers;
using UI.Mvc.Models;

namespace UI.Mvc.Controllers
{
    public class SearchController : Controller
    {
        private readonly IAuctionService<int> auctionService;

        public SearchController(IAuctionService<int> iau)
        {
            auctionService = iau;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }
        public async Task<ActionResult> Find(string query, int? page, int? catId)
        {
            int pageSize = 15;
            page = page ?? 1;
            SearchResult<LotModel> searchResult;
            if (string.IsNullOrEmpty(query))
            {
                IEnumerable<LotModel> emptyQueryResults;
                if (catId.HasValue)
                {
                    int cat = catId.Value;
                    ViewData["CatId"] = catId;
                    emptyQueryResults = (await auctionService.GetRange(0, pageSize, t => t.Categorie == cat)).Select(t => t.ToLotMvc());
                }
                else
                {
                    emptyQueryResults = (await auctionService.GetRange(0, pageSize)).Select(t => t.ToLotMvc());
                }
                if (emptyQueryResults.Count() != 0)
                {
                    searchResult = new SearchResult<LotModel>(emptyQueryResults, auctionService.Count(), 1);
                    return View(searchResult);
                }
                else
                {
                    ViewData["NoResults"] = "No results for your query.";
                    return View();
                }
            }

            DateTime now = DateTime.Now;
            var results = (await auctionService.GetRange((page.Value - 1) * pageSize, pageSize, t => t.Title.Contains(query) || t.Description.Contains(query) && t.AuctionEndDate >= now)).Select(t => t.ToLotMvc());
            if (results.Count() <= 0)
            {
                ViewData["NoResults"] = "No results for your query.";
                return View();
            }
            searchResult = new SearchResult<LotModel>(results, auctionService.Count(t => t.Title.Contains(query) || t.Description.Contains(query)), page.Value);
            return View(searchResult);
        }
    }
}