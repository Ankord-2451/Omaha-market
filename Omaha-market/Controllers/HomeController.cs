using Microsoft.AspNetCore.Mvc;
using Omaha_market.Data;
using Omaha_market.Core;
using System;
using Microsoft.AspNetCore.Http.Extensions;

namespace Omaha_market.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext dbContext;
        public HomeController(AppDbContext appDb)
        {
            dbContext = appDb;
        }

        public IActionResult Index()
        {
            
            return RedirectToAction("Lang",new {act = "Index", con="Market"});
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchstr,int page = 1)
        {
            var session = new SessionWorker(HttpContext);
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Search");
            returnP.Add("con", "Home");
            ViewBag.returnP = returnP;

            if(searchstr != null)
            {          
            var helper = new Helper();
            int AmountOfPages;

            if (page <= 0) page = 1;

            var products = helper.PageSplitHelper(await helper.FuzzySearchAsync(searchstr, dbContext.Products.ToList()), page, out AmountOfPages);

            if (page > AmountOfPages)
            {
                page = 1;
                products = helper.PageSplitHelper(await helper.FuzzySearchAsync(searchstr, dbContext.Products.ToList()), page, out AmountOfPages);
            }

            ViewData["Page"] = page;

            ViewData["AmountOfPages"] = AmountOfPages;

            ViewData["searchstr"] = searchstr;

                return View(products);
            }

            return View(null);           
        }

        [HttpGet("Lang/{act?}/{con?}/{id?}")]
        public IActionResult Lang(string act,string con,int id)
        {
            Dictionary<string,string> dic = new Dictionary<string,string>();
            var lang = dbContext.Lang.ToList();
            var session = new SessionWorker(HttpContext);
            if(session.IsRu())
            {
                foreach(var item in lang)
                {
                    dic.Add(item.Key, item.ValueRo);
                }
                session.SetLanguage("Ro");
                session.SetLangDic(dic);
            }
            else
            {
                foreach (var item in lang)
                {
                    dic.Add(item.Key, item.ValueRu);
                }
                session.SetLanguage("Ru");
                session.SetLangDic(dic);
            }
            if(id!=0) return RedirectToAction(act, con,new { id = id });

            return RedirectToAction(act,con);
        }

    }
}