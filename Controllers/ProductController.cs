using Microsoft.AspNetCore.Mvc;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Services.CategoryService;
using Poliak_UI_WT.Services.PhoneService;

namespace Poliak_UI_WT.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPhoneService _phoneService;

        public ProductController(ICategoryService categoryService, IPhoneService phoneService)
        {
            _categoryService = categoryService;
            _phoneService = phoneService;
        }

        public async Task<IActionResult> Index()
        {
            var productResponse = await _phoneService.GetPhoneListAsync(null);
            if (!productResponse.Success)
                return NotFound(productResponse.Error);
            return View(productResponse.Data.Items);
        }
    }
}
