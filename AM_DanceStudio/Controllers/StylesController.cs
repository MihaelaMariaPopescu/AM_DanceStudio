using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Mvc;

namespace AM_DanceStudio.Controllers
{
    public class StylesController : Controller
    {
        private readonly ApplicationDbContext db;
        public StylesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var styles = db.Styles;
            ViewBag.Styles = styles;
            return View();
        }
        public IActionResult New()
        {
            return View(); 
        }
        [HttpPost]

        public IActionResult New(Style stil)
        {
            try
            {
                db.Add(stil);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("New");
            }
           
        }
        public ActionResult Edit(int id)
        {
            Style styl = db.Styles.Find(id);
            ViewBag.Style = styl;
            return View();
        }

        [HttpPost]

        public ActionResult Edit(int id, Style style)
        {
            try
            {
                Style stil = db.Styles.Find(id);
                {
                    stil.Name = style.Name;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit");
            }
        }

        public ActionResult Delete(int id)
        {
            Style stil = db.Styles.Find(id);
            db.Styles.Remove(stil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
