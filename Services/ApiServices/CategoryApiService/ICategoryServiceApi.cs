using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;

namespace Poliak_UI_WT.Services.ApiServices.CategoryApiService
{
    public interface ICategoryServiceApi
    {
        public Task<ResponseData<List<Category>>> GetAllCategoryAsync();
    }
}
