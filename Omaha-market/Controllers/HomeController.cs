using Microsoft.AspNetCore.Mvc;
using Omaha_market.Data;
using Omaha_market.Core;
using Bogus.DataSets;

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
        public async Task<IActionResult> Search(string Name,int id = 1)
        {
            var session = new SessionWorker(HttpContext);
            if (session.GetLangDic() == null)
            {
                return RedirectToAction("Lang", new { act = "Search", con = "Home",name=Name ,id = id });
            }
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            ViewData["IsRu"] = session.IsRu();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Search");
            returnP.Add("con", "Home");
            returnP.Add("name", Name);
            ViewBag.returnP = returnP;

            if(Name != null)
            {          
            var helper = new Helper();
            int AmountOfPages;

            if (id <= 0) id = 1;

            var products = helper.PageSplitHelper(await helper.FuzzySearchAsync(Name, dbContext.Products.ToList()), id, out AmountOfPages);

            if (id > AmountOfPages)
            {
                id = 1;
                products = helper.PageSplitHelper(await helper.FuzzySearchAsync(Name, dbContext.Products.ToList()), id, out AmountOfPages);
            }

            ViewData["Page"] = id;

            ViewData["AmountOfPages"] = AmountOfPages;

            ViewData["searchstr"] = Name;

                return View(products);
            }

            return View(null);           
        }

        [HttpGet("Lang/{act?}/{con?}/{name?}/{id?}")]
        public IActionResult Lang(string act,string con,string name,int id)
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
            if (id != 0 && name != null) return RedirectToAction(act, con, new { Name = name , id = id });
            if (name != null) return RedirectToAction(act, con, new { Name = name });
            if (id!=0) return RedirectToAction(act, con,new { id = id });

            return RedirectToAction(act,con);
        }

    }
}