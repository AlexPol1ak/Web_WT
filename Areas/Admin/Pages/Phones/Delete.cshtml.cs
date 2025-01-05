using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Areas.Admin.Pages.Phones
{
    /// <summary>
    /// Страница удаления телефона.
    /// </summary>
    [Authorize(Policy = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IPhoneService _phoneService;

        public DeleteModel(IPhoneService phoneService)
        {
            _phoneService = phoneService;
        }

        [BindProperty]
        public Phone Phone { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _phoneService.GetPhoneByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                return NotFound("Phone not found.");
            }

            Phone = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Удаляем телефон через сервис
            var response = await _phoneService.DeletePhoneAsync(id);
            if (!response)
            {
                ModelState.AddModelError(string.Empty, "Error occurred while deleting the phone.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
