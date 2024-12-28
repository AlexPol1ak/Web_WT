using Microsoft.AspNetCore.Mvc;

namespace Poliak_UI_WT.ViewComponents
{
    /// <summary>
    /// Компонент корзины.
    /// </summary>
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    } 
}
