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

        public async Task<IActionResult> Index(string? category)
        {
            var categoriesResponse = await _categoryService.GetAllCategoryAsync();
            if(!categoriesResponse.Success)
                return NotFound(categoriesResponse.Error);      

            ViewData["categories"] = categoriesResponse.Data;
            var currentCategory = category == null ? "Все" :
                categoriesResponse.Data.FirstOrDefault(c =>c.NormalizedName == category)?.Name;
            ViewData["currentCategory"] = currentCategory;

            var phoneResponse = await _phoneService.GetPhoneListAsync(category);
            if (!phoneResponse.Success)
                return NotFound(phoneResponse.Error);

            return View(phoneResponse.Data.Items);
        }
    }
}
