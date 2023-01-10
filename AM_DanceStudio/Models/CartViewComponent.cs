using AM_DanceStudio.Data;
using AM_DanceStudio.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AM_DanceStudio.Models
{
    public class CartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
            ShoppingCart cart2;

            if(cart==null || cart.Count==0)
            {
                cart2 = null;
            }
            else
            {
                cart2 = new()
                {
                    NrItems = cart.Sum(x => x.Quantity),
                    TotalAmount = (int)cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(cart2);
        }
    }
}
