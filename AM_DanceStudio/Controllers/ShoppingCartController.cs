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

        public async Task< IActionResult> Add(int id)
        {
            Class cls = await context.Classes.FindAsync(id);

            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart") ?? new List<Item>();
           
            Item cartitem=cart.Where(c=> c.Id == id).FirstOrDefault();
            
            if(cartitem==null)
            {
                cart.Add(new Item(cls));
            }
            else
            {
                cartitem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "The product has been added";
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> Decrease(int id)
        {

            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
            Item cartitem = cart.Where(c => c.Id == id).FirstOrDefault();

            if (cartitem.Quantity>1)
            {
                --cartitem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.Id == id);
            }

            if(cart.Count==0)
            {

                HttpContext.Session.Remove("Cart");

            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(int id)
        {

            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
            cart.RemoveAll(p => p.Id == id);
          
            if (cart.Count == 0)
            {

                HttpContext.Session.Remove("Cart");

            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed";
            return RedirectToAction("Index");
        }


        public IActionResult Clear()
        {

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
