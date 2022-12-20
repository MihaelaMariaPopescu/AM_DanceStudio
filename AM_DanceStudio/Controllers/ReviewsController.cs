using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Mvc;

namespace AM_DanceStudio.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        public ReviewsController(ApplicationDbContext context)
        {
            db=context;
        }

        //Adaugarea unui comentariu in baza de date
        [HttpPost]
        public IActionResult New(Review rev)
        {
            rev.Date=DateTime.Now;

            try
            {
                db.Reviews.Add(rev);
                db.SaveChanges();
                return Redirect("/Classes/Show/" + rev.ClassId);
            }
            catch (Exception)
            {
                return Redirect("/Classes/Show/" + rev.ClassId);
            }

        }
        //Stergere review
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Review rev=db.Reviews.Find(id);
            db.Reviews.Remove(rev);
            db.SaveChanges();
            return Redirect("/Classes/Show/" + rev.ClassId);
        }

        //Editarea unui review existent
        
        public IActionResult Edit(int id)
        {
            Review rev = db.Reviews.Find(id);
            ViewBag.Review = rev;
            return View();
        }

        [HttpPost]
        public IActionResult Edit (int id, Review requestReview)
        {
            Review rev = db.Reviews.Find(id);
            try
            {
                rev.Text=requestReview.Text;
                db.SaveChanges();
                return Redirect("/Classes/Show/" + rev.ClassId);
            }
            catch (Exception e)
            {
                return Redirect("/Classes/Show/" + rev.ClassId);
            }
        }
    }
}
