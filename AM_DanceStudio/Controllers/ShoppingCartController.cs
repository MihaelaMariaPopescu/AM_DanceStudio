using AM_DanceStudio.Data;
using AM_DanceStudio.Models;
using AM_DanceStudio.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AM_DanceStudio.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartController(ApplicationDbContext c)
        {
            context = c;
        }
        public IActionResult Index()
        {
            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart")?? new List<Item>();
            CartViewModel cartvm = new()
            {
                Items = cart,
                GrandTotal = (int)cart.Sum(x => x.Quantity * x.Price)
            };
            return View(cartvm);
        }

      /*  public IActionResult AddtoCart()
        {
            if (SessionOptions["cart"]==null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item());
            }
            else
            {

            }
            return View();
        }*/
    }
}
