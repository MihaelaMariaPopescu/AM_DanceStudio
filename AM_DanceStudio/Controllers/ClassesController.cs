﻿using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
        //Se afiseaza lista tuturor claselor din baza de date impreuna cu categoria din care fac parte
        //acesta e proiectul final updatat
        public IActionResult Index()
        {
            var classes = db.Classes.Include("Instructor").Include("Style").Include("Studio").Include("User");

            ViewBag.Classes = classes;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }
        //Se afiseaza un singur articol in functie de id-ul sau
        [Authorize(Roles = "User,Colaborator,Admin")]

        public IActionResult Show(int id)
        {


            Class classe = db.Classes.Include("Style").
                                        Include("Instructor")
                                        .Include("Studio")
                                        .Include("Reviews")
                                        .Include("User")   
                                        .Where(art => art.Id == id).First();

            ViewBag.Class = classe;
            ViewBag.Style = classe.Style;
            ViewBag.Instructor = classe.Instructor;
            ViewBag.Studio = classe.Studio;

            SetAccessRights();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show([FromForm] Review comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);
            /*
            if (ModelState.IsValid)
            {
                db.Reviews.Add(comment);
                db.SaveChanges();
                return Redirect("/Articles/Show/" + comment.ClassId);
            }

            else
            {
            */
                Class art = db.Classes.Include("Style").
                                            Include("Instructor")
                                            .Include("Studio")
                                            .Include("Reviews")
                                            .Include("User")
                                            .Where(art => art.Id == comment.ClassId).First();

                //return Redirect("/Articles/Show/" + comm.ArticleId);

                SetAccessRights();

                return View(art);
           // }
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
        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult New(Class clasa)
        {
            clasa.UserId = _userManager.GetUserId(User);

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

        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult Edit(int id)
        {
            Class classe = db.Classes.Include("Instructor").Include("Style").Include("Studio")
                .Where(art => art.Id == id).First();

            ViewBag.Class = classe;
            ViewBag.Style = classe.Style;
            ViewBag.Instructor = classe.Instructor;
            ViewBag.Studio = classe.Studio;

            var styless = from styles in db.Styles
                          select styles;
            ViewBag.Styles = styless;

            var instructorss = from instructors in db.Instructors
                               select instructors;
            ViewBag.Instructors = instructorss;


            var studioss = from studios in db.Studios
                           select studios;
            ViewBag.Studios = studioss;
            if (classe.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View();
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

            try
            {
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
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei clase care nu va apartine";
                        return RedirectToAction("Index");
                    }
                }
            }

            catch(Exception)
            {
                return RedirectToAction("Edit", id);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator,Admin")]
        public IActionResult Delete(int id) { 
            Class clasa = db.Classes.Include("Instructor").Include("Style").Include("Studio").Where(art => art.Id == id).First();
            if (clasa.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                  db.Classes.Remove(clasa);
                  db.SaveChanges();
                TempData["message"] = "Articolul a fost sters";  
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o clasa care nu va apartine";
                return RedirectToAction("Index");
            }

        }
    }
}
