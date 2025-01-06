using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Areas.Admin.Pages.Phones
{
    public class DetailModel : PageModel
    {
        private readonly IPhoneService _phoneService;
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public Phone Phone { get; set; } = default!;

        public DetailModel(IPhoneService phoneService, ICategoryService categoryService)
        {
            _phoneService = phoneService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _phoneService.GetPhoneByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                return NotFound("Phone not found.");
            }

            Phone = response.Data;

            var responseCategories = await _categoryService.GetAllCategoryAsync();
            if (responseCategories.Success || responseCategories.Data.Count > 0)
            {
                Category? cat = responseCategories.Data.Find(c => c.CategoryId == Phone.CategoryId);
                if (cat != null) Phone.Category = cat;
            }

            return Page();
        }
    }
}
