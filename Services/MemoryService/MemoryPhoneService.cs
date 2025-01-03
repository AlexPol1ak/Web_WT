using Microsoft.AspNetCore.Mvc;
using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;
using Poliak_UI_WT.Services.Interfaces;

namespace Poliak_UI_WT.Services.MemoryService
{
    /// <summary>
    /// Тестовое хранилище телефонов.
    /// </summary>
    public class MemoryPhoneService : IPhoneService
    {
        List<Phone> _phones;
        List<Category> _categories;
        IConfiguration _configuration;

        public MemoryPhoneService
            ([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetAllCategoryAsync().Result.Data;
            _phones = new List<Phone>();
            _configuration = config;
            SetupData();
        }

        private void SetupData()
        {
            Category iosCat = _categories.Find(name => name.Name == "iOS");
            Category androidCat = _categories.Find(name => name.Name == "Android");

            Phone s24Black = new()
            {
                PhoneId = 1,
                Name = "Samsung",
                Model = "S24 Black",
                Price = 3200,
                Image = "Images\\MemoryPhones\\s24black.jpg",
                Category = androidCat
            };

            Phone s24Gold = new()
            {
                PhoneId = 2,
                Name = "Samsung",
                Model = "S24 Gold",
                Price = 3100,
                Image = "Images\\MemoryPhones\\s24gold.jpg",
                Category = androidCat
            };

            Phone s24ultrablack = new()
            {
                PhoneId = 3,
                Name = "Samsung",
                Model = "S24 Ultra Black",
                Price = 4300,
                Image = "Images\\MemoryPhones\\s24ultrablack.jpg",
                Category = androidCat
            };

            Phone s24ultragold = new()
            {
                PhoneId = 4,
                Name = "Samsung",
                Model = "S24 Ultra Gold",
                Price = 3200,
                Image = "Images\\MemoryPhones\\s24ultragold.jpg",
                Category = androidCat
            };

            Phone ip15problack = new()
            {
                PhoneId = 5,
                Name = "iPhone",
                Model = "15 pro black",
                Price = 2900,
                Image = "Images\\MemoryPhones\\iphone15problack.jpg",
                Category = iosCat
            };

            Phone ip15prodeserttitan = new()
            {
                PhoneId = 6,
                Name = "iPhone",
                Model = "15 pro desert titanium",
                Price = 3100,
                Image = "Images\\MemoryPhones\\iphone15prodeserttitan.jpg",
                Category = iosCat
            };

            Phone ip15prorose = new()
            {
                PhoneId = 7,
                Name = "iPhone",
                Model = "15 pro rose",
                Price = 3100,
                Image = "Images\\MemoryPhones\\iphone15prorose.jpg",
                Category = iosCat
            };
            Phone ip15protitan = new()
            {
                PhoneId = 8,
                Name = "iPhone",
                Model = "15 pro titanium",
                Price = 3200,
                Image = "Images\\MemoryPhones\\iphone15protitan.jpg",
                Category = iosCat
            };

            List<Phone> newPhone = new() {s24Black, s24Gold, s24ultrablack, s24ultragold, ip15problack,
                ip15prodeserttitan, ip15prorose, ip15protitan
            };
            _phones.AddRange(newPhone);

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

        public Task UpdatePhoneAsync(int id, Phone phone, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает  список смартфонов
        /// </summary>
        /// <param name="categoryNormalizedName">Имя категории</param>
        /// <param name="pageNo">Необходимая тсраница</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Phone>>> GetPhoneListAsync(string? categoryNormalizedName, int pageNo = 1)
        {

            //Console.WriteLine($"_________________________categoryNormalizedName: {categoryNormalizedName}");

            int pageSize = _configuration.GetSection("ItemsPerPage").Get<int>();
            var result = new ResponseData<ListModel<Phone>>();
            int? categoryId = null;
            List<Phone> phonesData = new();

            if (categoryNormalizedName != null)
            {
                categoryId = _categories.Find(
                    c => c.NormalizedName == categoryNormalizedName
                    )?.CategoryId;

                phonesData = _phones.Where(p => p.Category != null && p.Category.CategoryId == categoryId)?.ToList();

            }
            else
            {
                phonesData = _phones;
            }

            int totalPages = (int)Math.Ceiling(phonesData.Count / (double)pageSize);

            var listData = new ListModel<Phone>()
            {
                Items = phonesData.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages,
            };

            result.Data = listData;

            if (phonesData.Count == 0)
            {
                result.Success = false;
                result.Error = "Нет объектов в выбранной категории";
            }

            return Task.FromResult(result);
        }
    }
}
