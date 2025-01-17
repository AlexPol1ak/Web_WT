﻿using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;

namespace Poliak_UI_WT.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса телефонов.
    /// </summary>
    public interface IPhoneService
    {
        public Task<ResponseData<ListModel<Phone>>> GetPhoneListAsync
            (string? categoryNormalizedName, int pageNo = 1);

        public Task<ResponseData<Phone>> GetPhoneByIdAsync(int id);

        public Task UpdatePhoneAsync(int id, Phone phone, IFormFile? formFile);

        public Task<bool> DeletePhoneAsync(int id);

        public Task<ResponseData<Phone>> CreatePhoneAsync(Phone phone, IFormFile? formFile);
    }
}
