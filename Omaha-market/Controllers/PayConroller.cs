using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Omaha_market.Controllers
{
    public class PayConroller : Controller
    {
        // GET: PayConroller
        public ActionResult Index()
        {
            return View();
        }

        // GET: PayConroller/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PayConroller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayConroller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PayConroller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PayConroller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PayConroller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PayConroller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
