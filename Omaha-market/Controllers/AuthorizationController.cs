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
            if (session.IsAuthorized())
            {
                return View("Auth",session.GetUserName());
            }
             return View();
        }

        [HttpPost("Authorization/Form")]
        public ActionResult Index(string login,string password)
        {
            AccountModel account;

          
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
                //Object for work with session
                var session = new SessionWorker(HttpContext);

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
            return StatusCode(401);
        }



        [HttpPost("Authorization/LogOut")]
        public ActionResult LogOut()
        {
            var session = new SessionWorker(HttpContext);
            session.Clear();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Registration/Form")]
        public ActionResult Regist()
        {
            return View();
        }

        [HttpPost("Registration/Form")]
        public ActionResult Regist(AccountModel account)
        {
            account.Role = role.Customer;
            account.Password = Encoder.Encode(configuration, account.Password);
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
