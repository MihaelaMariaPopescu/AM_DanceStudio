using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AM_DanceStudio.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public ReviewsController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Adaugarea unui comentariu in baza de date
        /*
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
        */
        //Stergere review
        [HttpPost]
        [Authorize(Roles = "User,Admin,Colaborator")]
        public IActionResult Delete(int id)
        {
            Review rev = db.Reviews.Find(id);

            if (rev.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();
                return Redirect("/Classes/Show/" + rev.ClassId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti comentariul";
                return RedirectToAction("Index", "Classes");
            }

        }

        //Editarea unui review existent
        
        public IActionResult Edit(int id)
        {
            Review rev = db.Reviews.Find(id);
            if (rev.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                ViewBag.Review = rev;
                    return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati comentariul";
                return RedirectToAction("Index", "Classes");
            }
           
        }

        [HttpPost]
        public IActionResult Edit (int id, Review requestReview)
        {
            Review rev = db.Reviews.Find(id);
            try
            {
                if (rev.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    rev.Text = requestReview.Text;
                    db.SaveChanges();
                    return Redirect("/Classes/Show/" + rev.ClassId);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa editati comentariul";
                    return RedirectToAction("Index", "Classes");
                }
            }
            catch (Exception e)
            {
                return Redirect("/Classes/Show/" + rev.ClassId);
            }
        }
    }
}
