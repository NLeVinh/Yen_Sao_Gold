using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using gold_server.DTOs.Province;

namespace gold_server.Services
{
    public interface IProvinceWardImportService
    {
        Task ImportFromJsonAsync(Stream jsonStream);
        Task<List<ProvinceDto>> GetAllProvincesAsync();
        Task<ProvinceDto> GetProvinceByIdAsync(int id);
    }
}