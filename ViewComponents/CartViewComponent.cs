using Microsoft.AspNetCore.Mvc;

namespace Poliak_UI_WT.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    } 
}
