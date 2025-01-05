using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;
using Poliak_UI_WT.Services.Interfaces;
using System.Text.Json;

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

        /// <summary>
        /// Создать новый телефон.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<ResponseData<Phone>> CreatePhoneAsync(Phone phone, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Подготовить объект, возвращаемый методом
            var responseData = new ResponseData<Phone>();
            // Послать запрос к API для сохранения объекта
            var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress, phone);

            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.Error = $"Не удалось создать объект: {response.StatusCode}";
                return responseData;
            }

            if (formFile != null)
            {
                // получить созданный объект из ответа Api - сервиса
                var responsePhone = await response.Content.ReadFromJsonAsync<Phone>();

                // создать объект запроса
                Uri requestUri = new Uri($"{_httpClient.BaseAddress}{responsePhone.PhoneId}");
                //DebugHelper.ShowData(requestUri.ToString(), "requestUri");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = requestUri
                };
                // Создать контент типа multipart form-data
                var content = new MultipartFormDataContent();
                // создать потоковый контент из переданного файла
                var streamContent = new StreamContent(formFile.OpenReadStream());
                // добавить потоковый контент в общий контент по именем "image"
                content.Add(streamContent, "image", formFile.FileName);
                // поместить контент в запрос
                request.Content = content;
                // послать запрос к Api-сервису
                response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.Error = $"Не удалось сохранить изображение:{response.StatusCode}";
                }

            }
            return responseData;
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
                //DebugHelper.ShowError(result.Content.ToString(), "Сообщение");
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
