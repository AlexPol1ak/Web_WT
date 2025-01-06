using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Areas.Admin.Pages.Phones
{
    /// <summary>
    /// Модель редактирования телефона.
    /// </summary>
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPhoneService _phoneService;

        [BindProperty]
        public Phone Phone { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }

        public EditModel(ICategoryService categoryService, IPhoneService phoneService)
        {
            _categoryService = categoryService;
            _phoneService = phoneService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _phoneService.GetPhoneByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                return NotFound("Phone not found.");
            }

            Phone = response.Data;

            var categoryListData = await _categoryService.GetAllCategoryAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "CategoryId", "Name", Phone.CategoryId);
            //DebugHelper.ShowData("OnGetAsync", id, Phone.PhoneId, Phone.Name, Phone.Model, Phone.Price);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categoryListData = await _categoryService.GetAllCategoryAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "CategoryId", "Name", Phone.CategoryId);
                //DebugHelper.ShowError("!ModelState.IsValid");
                return Page();
            }
            //DebugHelper.ShowData("OnPostAsync", Phone.PhoneId, Phone.Name, Phone.Model, Phone.Price);
            await _phoneService.UpdatePhoneAsync(Phone.PhoneId, Phone, Image);

            return RedirectToPage("./Index");
        }
    }
}
