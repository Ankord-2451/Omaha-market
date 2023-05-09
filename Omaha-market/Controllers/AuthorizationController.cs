using Microsoft.AspNetCore.Mvc;
using Omaha_market.Core;
using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Controllers
{
    public class AuthorizationController : Controller
    {

        private AppDbContext dbContext;
        public static IConfiguration configuration;

        public AuthorizationController(AppDbContext _dbContext, IConfiguration _configuration)
        {
            dbContext = _dbContext;
            configuration = _configuration;
        }

        [HttpGet("Authorization/Form")]
        public ActionResult Index()
        {
            var session = new SessionWorker(HttpContext);
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Index");
            returnP.Add("con", "Authorization");

            ViewBag.returnP = returnP;

            if (session.IsAuthorized())
            {
                return RedirectToAction("Index","Users",dbContext.Accounts.First(x=>x.ID==session.GetUserId()));
            }
             return View();
        }

        [HttpPost("Authorization/Form")]
        public ActionResult Index(string login,string password)
        {
            AccountModel? account;
            
            var session = new SessionWorker(HttpContext);

          
           password = Encoder.Encode(configuration, password);

            try { 
            account = dbContext.Accounts.First(e => (e.Login == login) && (e.Password == password));
            }
            catch
            {
                account = null;
            }

            if(account != null) 
            {
                //Set in session JWT Token for Authorization
                var Geterator = new GeneratorJWTTokens(configuration);
                var token = Geterator.GenerateJWTToken(account);
                
                session.SaveToken(token);
                //Set in session object type of AuthUserModel for Authentication
                session.SaveUserModel(new AuthorizedUser()
                { 
                    ID = account.ID,
                    name = account.Name,
                    role = account.Role 
                });

                return RedirectToAction(nameof(Index));
            } 
            
            if(session.IsRu())
            {
                ViewData["Eror"] = "неверный пороль или логин"; 
            }
            else
            {
                ViewData["Eror"] = "parolă sau autentificare nevalidă";
            }
           
            ViewBag.Lang = session.GetLangDic();
            ViewData["Language"] = session.GetLanguage();
            var returnP = new Dictionary<string, string>();
            returnP.Add("act", "Index");
            returnP.Add("con", "Authorization");

            ViewBag.returnP = returnP;

            return View("Index");
        }



        [HttpGet("Authorization/LogOut")]
        public ActionResult LogOut()
        {
            var session = new SessionWorker(HttpContext);
            session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
