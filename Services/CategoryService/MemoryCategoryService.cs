using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;

namespace Poliak_UI_WT.Services.CategoryService
{
    /// <summary>
    /// Тестовое хранилище категорий.
    /// </summary>
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetAllCategoryAsync()
        {
            var categories = new List<Category>()
            {
                new Category() {CategoryId=1, Name="iOS", NormalizedName="ios"},
                new Category() {CategoryId=2, Name="Android", NormalizedName="android"}
            };

            ResponseData<List<Category>> result = new(categories, true, null);
            return Task.FromResult(result);

        }

    }
}
