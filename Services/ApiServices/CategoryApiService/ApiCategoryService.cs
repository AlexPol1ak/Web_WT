using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;
using Poliak_UI_WT.Services.ApiServices.CategoryApiService;

namespace Poliak_UI_WT.Services.ApiService.CategoryApiService
{
    public class ApiCategoryService : ICategoryServiceApi
    {
        HttpClient _httpClient;

        public ApiCategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseData<List<Category>>> GetAllCategoryAsync()
        {
            var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
            };
            var response = new ResponseData<List<Category>>
            { Success = false, Error = "Ошибка чтения API" };
            return response;
        }
    }
}
