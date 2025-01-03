using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Services.ApiServices
{
    public class ApiPhoneService : IPhoneService
    {

        private readonly HttpClient _httpClient;

        // Конструктор принимает HttpClient, который будет предоставлен DI контейнером
        public ApiPhoneService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<ResponseData<Phone>> CreatePhoneAsync(Phone phone, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeletePhoneAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Phone>> GetPhoneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Phone>>> GetPhoneListAsync(
            string? categoryNormalizedName, int pageNo = 1)
        {
            var uri = _httpClient.BaseAddress;
            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString());
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName);
            }
            var query = QueryString.Create(queryData);

            var result = await _httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine(await result.Content.ReadAsStringAsync());
                return await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Phone>>>();
            };
            var response = new ResponseData<ListModel<Phone>>
            { Success = false, Error = "Ошибка чтения API" };
            return response;
        }

        public Task UpdatePhoneAsync(int id, Phone phone, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
