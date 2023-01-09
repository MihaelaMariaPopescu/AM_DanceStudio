using Microsoft.AspNetCore.Mvc;

namespace AM_DanceStudio.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
