using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Poliak_UI_WT.Models;

namespace Poliak_UI_WT.Controllers
{
    /// <summary>
    /// Контроллер домашней страницы.
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("TitleLab", "Лабораторная работа 2");

            var items = new List<ListDemo>
        {
            new ListDemo { Id = 1, Name = "Item 1" },
            new ListDemo { Id = 2, Name = "Item 2" },
            new ListDemo { Id = 3, Name = "Item 3" }
        };
            var selectList = new SelectList(items, "Id", "Name");
            ViewBag.Items = selectList;

            return View();
        }
    }
}
