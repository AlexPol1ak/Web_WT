using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;
using Poliak_UI_WT.Domain.Utils;
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

        /// <summary>
        /// Удаляет телефон по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePhoneAsync(int id)
        {

            Uri requestUri = new Uri($"{_httpClient.BaseAddress}{id}");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = requestUri
            };

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode) return true;
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                //DebugHelper.ShowData($"Failed to delete phone with ID {id}. Response: {errorContent}");
                return false;
            }

        }

        /// <summary>
        /// Получить телефон по Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseData<Phone>> GetPhoneByIdAsync(int id)
        {
            ResponseData<Phone> responseData = new ResponseData<Phone>();

            Uri requestUri = new Uri($"{_httpClient.BaseAddress}{id}");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri
            };

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responsePhone = await response.Content.ReadFromJsonAsync<Phone>();
                if (responsePhone != null)
                {
                    responseData.Success = true;
                    responseData.Data = responsePhone;
                    return responseData;
                }
                else
                {
                    responseData.Success = false;
                    responseData.Error = "Не удалось обработать данные.";
                    //DebugHelper.ShowError(responseData.Error);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Error = "Не удалось получить телефон.";
                //DebugHelper.ShowError(responseData.Error);
            }
            return responseData;
        }

        /// <summary>
        /// Получить список телефонов.
        /// </summary>
        /// <param name="categoryNormalizedName"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновляет данные телефона.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdatePhoneAsync(int id, Phone phone, IFormFile? formFile)
        {
            var responseData = new ResponseData<Phone>();
            DebugHelper.ShowData("UpdatePhoneAsync", id, phone.Name, phone.Model, phone.Price);

            if (id != phone.PhoneId)
            {
                responseData.Success = false;
                responseData.Error = "ID в запросе не совпадает с ID телефона.";
                return;
            }

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Отправляем PUT-запрос на обновление телефона
            var response = await _httpClient.PutAsJsonAsync($"{_httpClient.BaseAddress}{id}", phone);

            if (!response.IsSuccessStatusCode)
            {

                responseData.Success = false;
                responseData.Error = $"Не удалось обновить телефон: {response.StatusCode}";
                //DebugHelper.ShowError(responseData.Error);
                return;
            }

            // Если передан новый файл изображения
            if (formFile != null)
            {
                Uri requestUri = new Uri($"{_httpClient.BaseAddress}update_image/{id}");
                DebugHelper.ShowData(requestUri);
                var imageRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = requestUri
                };

                // Создаем multipart/form-data контент
                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(formFile.OpenReadStream());
                content.Add(streamContent, "image", formFile.FileName);

                imageRequest.Content = content;

                var imageResponse = await _httpClient.SendAsync(imageRequest);

                if (!imageResponse.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.Error = $"Не удалось сохранить изображение: {imageResponse.StatusCode}";
                    DebugHelper.ShowError(responseData.Error);
                    return;
                }
            }
            return;
        }
    }
}
