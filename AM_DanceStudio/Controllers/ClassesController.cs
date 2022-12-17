using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AM_DanceStudio.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext db;
        public ClassesController(ApplicationDbContext context) 
        {
            db = context;
        }
//Se afiseaza lista tuturor claselor din baza de date impreuna cu categoria din care fac parte
//acesta e proiectul final updatat
        public IActionResult Index()
        {
           var classes= db.Classes.Include("Instructor").Include("Style").Include("Studio");

            ViewBag.Classes = classes;

            return View();
        }
        //Se afiseaza un singur articol in functie de id-ul sau

        public IActionResult Show(int id)
        {
            Class classe = db.Classes.Include("Instructor").Include("Style").Include("Studio")
                            .Where(art => art.Id == id).First();

            ViewBag.Class = classe;
            ViewBag.Style = classe.Style;
            ViewBag.Instructor = classe.Instructor;
            ViewBag.Studio = classe.Studio;
            return View();
        }

        //Se afiseaza formularul in care se vor completa datele unei clase
        //Impreuna cu selectarea stilului, studioului si instructorului care o va tine

        public IActionResult New() {
            var styless = from styles in db.Styles
                         select styles;
            ViewBag.Styles = styless;

            var instructorss = from instructors in db.Instructors
                               select instructors;
            ViewBag.Instructors = instructorss;


            var studioss = from studios in db.Studios
                           select studios;
            ViewBag.Studios = studioss;
            return View();
        }

      [HttpPost]

        public IActionResult New(Class clasa)
        {
            try
            {
                db.Classes.Add(clasa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("New");
            }
        }
    }
}
