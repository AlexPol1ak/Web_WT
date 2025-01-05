using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Areas.Admin.Pages.Phones
{
    /// <summary>
    /// Страница создания телефона.
    /// </summary>
    [Authorize(Policy = "admin")]
    public class CreateModel : PageModel
    {
        ICategoryService _categoryService;
        IPhoneService _phoneService;

        [BindProperty]
        public Phone Phone { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }

        public CreateModel(ICategoryService categoryService, IPhoneService phoneService)
        {
            _categoryService = categoryService;
            _phoneService = phoneService;
        }

        public async Task<IActionResult> OnGet()
        {
            var categoryListData = await _categoryService.GetAllCategoryAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "CategoryId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _phoneService.CreatePhoneAsync(this.Phone, Image);

            return RedirectToPage("./Index");
        }

    }
}
