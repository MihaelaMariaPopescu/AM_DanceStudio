using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Xml.Schema;

namespace AM_DanceStudio.Controllers
{
    [Authorize(Roles = "User,Colaborator,Admin")]
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public ClassesController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Index(string SearchString)
        {
            ViewData["CurrentFilter"] = SearchString;
            var classess = db.Classes.Include("Instructor").Include("Style").Include("Studio").Include("User");
            bool ok = false;
            ViewBag.ok = false;
            if (!String.IsNullOrEmpty(SearchString))
            {
                classess = classess.Where(b => b.Name.Contains(SearchString));
                ok = true;
                ViewBag.ok = true;
            }
            ViewBag.Clasa = classess;

            var classes = db.Classes.Include("Instructor").Include("Style").Include("Studio").Include("User");



            ViewBag.Classes = classes;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult order()
        {
            var clasele = from clasa in db.Classes.Include("Instructor").Include("Style").Include("Studio").Include("User")
                          orderby clasa.Price descending
                          select clasa;

            ViewBag.ClasaDesc = clasele;

            return View();
        }

        [AllowAnonymous]
        public IActionResult order1()
        {
            var clasele = from clasa in db.Classes.Include("Instructor").Include("Style").Include("Studio").Include("User")
                          orderby clasa.Price 
                          select clasa;

            ViewBag.ClasaCresc = clasele;

            return View();
        }
        //Se afiseaza un singur articol in functie de id-ul sau
        [Authorize(Roles = "User,Colaborator,Admin")]
        [AllowAnonymous]
        public IActionResult Show(int id)
        {


            Class classe = db.Classes.Include("Style").
                                        Include("Instructor")
                                        .Include("Studio")
                                        .Include("Reviews")
                                        .Include("User")
                                        .Include("Reviews.User")
                                        .Where(art => art.Id == id).First();

            ViewBag.Class = classe;
            ViewBag.Style = classe.Style;
            ViewBag.Instructor = classe.Instructor;
            ViewBag.Studio = classe.Studio;
            ViewBag.Reviews = classe.Reviews;

            SetAccessRights();

            return View(classe);
        }

        [HttpPost]
        [Authorize(Roles = "User,Colaborator,Admin")]
        public IActionResult Show([FromForm] Review comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                db.Reviews.Add(comment);
                db.SaveChanges();
                return Redirect("/Classes/Show/" + comment.ClassId);
            }
            else
            {
                Class art = db.Classes.Include("Style").
                                            Include("Instructor")
                                            .Include("Studio")
                                            .Include("Reviews")
                                            .Include("User")
                                            .Include("Reviews.User")
                                            .Where(art => art.Id == comment.ClassId).First();
                db.Reviews.Add(comment);
                db.SaveChanges();
                return Redirect("/Classes/Show/" + comment.ClassId);
                // SetAccessRights();

                //return View(art);
            }
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Colaborator"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);

            ViewBag.EsteAdmin = User.IsInRole("Admin");
        }

        //Se afiseaza formularul in care se vor completa datele unei clase
        //Impreuna cu selectarea stilului, studioului si instructorului care o va tine

        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult New()
        {

            Class cls = new Class();

            cls.Stil = GetAllStyles();

            cls.Ins = GetAllInstructors();

            cls.St = GetAllStudios();
            return View(cls);
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult New(Class clasa)
        {
            clasa.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Classes.Add(clasa);
                db.SaveChanges();
                TempData["message"] = "Cursul a fost adaugat";
                return RedirectToAction("Index");
            }
            else
            {
                clasa.Stil = GetAllStyles();

                clasa.Ins = GetAllInstructors();

                clasa.St = GetAllStudios();
                return View(clasa);
            }
        }

        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult Edit(int id)
        {
            Class classe = db.Classes.Include("Instructor").Include("Style").Include("Studio")
                .Where(art => art.Id == id).First();
            classe.Stil = GetAllStyles();

            classe.Ins = GetAllInstructors();

            classe.St = GetAllStudios();

            if (classe.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(classe);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei clase care nu va apartine";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin")]

        public IActionResult Edit(int id, Class requestClass)
        {
            Class clasa = db.Classes.Find(id);

            if (ModelState.IsValid)
            {

                if (clasa.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    clasa.Name = requestClass.Name;
                    clasa.Description = requestClass.Description;
                    clasa.Picture = requestClass.Picture;
                    clasa.StyleId = requestClass.StyleId;
                    clasa.Price = requestClass.Price;
                    clasa.StudioId = requestClass.StudioId;
                    clasa.InstructorId = requestClass.InstructorId;
                    clasa.Rating = requestClass.Rating;
                    TempData["message"] = "Cursul a fost modificat";
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei clase care nu va apartine";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                requestClass.Stil = GetAllStyles();
                requestClass.Ins = GetAllInstructors();
                requestClass.St = GetAllStudios();
                return View(requestClass);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult Delete(int id)
        {
            Class clasa = db.Classes.Include("Instructor").Include("Style").Include("Studio").Where(art => art.Id == id).First();
            if (clasa.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Classes.Remove(clasa);
                db.SaveChanges();
                TempData["message"] = "Cursul a fost sters";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o clasa care nu va apartine";
                return RedirectToAction("Index");
            }

        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllStyles()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var styless = from style in db.Styles
                          select style;

            // iteram prin categorii
            foreach (var st in styless)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = st.Name.ToString()
                });
            }

            return selectList;
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllInstructors()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var i = from ins in db.Instructors
                    select ins;

            // iteram prin categorii
            foreach (var it in i)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = it.Id.ToString(),
                    Text = it.Name.ToString()
                });
            }

            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllStudios()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var studio1 = from st in db.Studios
                          select st;

            // iteram prin categorii
            foreach (var studio in studio1)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = studio.Id.ToString(),
                    Text = studio.Name.ToString()
                });
            }

            return selectList;
        }
        [Authorize(Roles = "Colaborator,Admin")]

        public IActionResult Admin()
        {
            var classes = db.Classes.Include("Instructor").Include("Style").Include("Studio").Include("User");

            ViewBag.Classes = classes;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        public IActionResult Accept(int id)
        {
            Class classe = db.Classes.Include("Instructor").Include("Style").Include("Studio")
                .Where(art => art.Id == id).First();

            classe.Valid = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
