using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AM_DanceStudio.Controllers
{
        [Authorize(Roles = "Admin")]
        public class UsersController : Controller
        {
            private readonly ApplicationDbContext db;

            private readonly UserManager<ApplicationUser> _userManager;

            private readonly RoleManager<IdentityRole> _roleManager;

            public UsersController(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager
                )
            {
                db = context;

                _userManager = userManager;

                _roleManager = roleManager;
            }
            public IActionResult Index()
            {
                var users = from user in db.Users
                            orderby user.UserName
                            select user;

                ViewBag.UsersList = users;

                return View();
            }

            public async Task<ActionResult> Show(string id)
            {
                ApplicationUser user = db.Users.Find(id);
                var roles = await _userManager.GetRolesAsync(user);

                ViewBag.Roles = roles;

                return View(user);
            }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = db.Users
                         .Include("Classes")
                         .Include("Reviews")
                         .Where(u => u.Id == id)
                         .First();

           
            if (user.Classes.Count > 0)
            {
                foreach (var article in user.Classes)
                {
                    db.Classes.Remove(article);
                }
            }
         
            if (user.Reviews.Count > 0)
            {
                foreach (var comment in user.Reviews)
                {
                    db.Reviews.Remove(comment);
                }
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user); 

            
            var currentUserRole = _roleManager.Roles
                                              .Where(r => roleNames.Contains(r.Name))
                                              .Select(r => r.Id)
                                              .First(); 
            ViewBag.UserRole = currentUserRole;

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();


            if (ModelState.IsValid)
            {
                user.UserName = newData.UserName;
                user.Email = newData.Email;
                user.FirstName = newData.FirstName;
                user.LastName = newData.LastName;
                user.PhoneNumber = newData.PhoneNumber;


             
                var roles = db.Roles.ToList();

                foreach (var role in roles)
                {
                    
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
              
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

    }
}
