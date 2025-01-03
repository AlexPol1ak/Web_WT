using Microsoft.AspNetCore.Mvc.RazorPages;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Areas.Admin.Pages.Phones
{
    public class IndexModel : PageModel
    {
        private readonly IPhoneService _productService;
        public IndexModel(IPhoneService productService)
        {
            //_context = context;
            _productService = productService;
        }

        public List<Phone> Phone { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public async Task OnGetAsync(int? pageNo = 1)
        {
            var response = await _productService.GetPhoneListAsync(null, pageNo.Value);
            if (response.Success)
            {
                Phone = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }

    }
}
