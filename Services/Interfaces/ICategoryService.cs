using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;

namespace Poliak_UI_WT.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса категорий.
    /// </summary>
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetAllCategoryAsync();
    }
}
