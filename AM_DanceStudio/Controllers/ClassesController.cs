using AM_DanceStudio.Data;
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
    }
}
