using Microsoft.AspNetCore.Mvc;

namespace Poliak_UI_WT.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
