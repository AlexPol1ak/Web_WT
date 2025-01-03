using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;
using Poliak_UI_WT.Services.Interfaces;


namespace Poliak_UI_WT.Services.ApiServices
{
    public class ApiCategoryService : ICategoryService
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
                var s = await result.Content.ReadFromJsonAsync<List<Category>>();
                ResponseData<List<Category>> rd = new();
                rd.Success = true;
                rd.Data = s.ToList();
                return rd;
            };
            var response = new ResponseData<List<Category>>
            { Success = false, Error = "Ошибка чтения API" };
            return response;
        }
    }
}
