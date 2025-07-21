using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using gold_server.DTOs.Province;
using gold_server.Models;
using Microsoft.EntityFrameworkCore;

namespace gold_server.Services
{
    public class ProvinceWardImportService : IProvinceWardImportService
    {
        private readonly GoldServerContext _context;
        public ProvinceWardImportService(GoldServerContext context)
        {
            _context = context;
        }

        // List all provinces with their wards
        public async Task<List<ProvinceDto>> GetAllProvincesAsync()
        {
            try
            {
                var provinces = await _context.PROVINCEs
                    .Include(p => p.WARDs)
                    .ToListAsync();

                var provinceDtos = provinces.Select(p => new ProvinceDto
                {
                    ID_Province = p.ID_Province,
                    Name = p.Name,
                    Description = p.Description,
                    Wards = p.WARDs.Select(w => new WardDto
                    {
                        ID_Ward = w.ID_Ward,
                        Name = w.Name,
                        Description = w.Description,
                        ID_Province = w.ID_Province
                    }).ToList()
                }).ToList();

                return provinceDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving province list: {ex.Message}");
            }
        }

        public async Task ImportFromJsonAsync(Stream jsonStream)
        {
            try
            {
                if (jsonStream == null || jsonStream.Length == 0)
                {
                    var defaultFilePath = Path.Combine(AppContext.BaseDirectory, "data", "provincesData.json");
                    if (!File.Exists(defaultFilePath))
                        throw new Exception("No uploaded file and default JSON file not found.");

                    jsonStream = new FileStream(defaultFilePath, FileMode.Open, FileAccess.Read);
                }

                var records = await JsonSerializer.DeserializeAsync<List<ProvinceWardJsonRecord>>(jsonStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (records == null || records.Count == 0)
                    throw new Exception("The JSON file is empty or invalid.");

                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

                var provinceDtos = records
                    .Where(r => !string.IsNullOrWhiteSpace(r.TenTinh) && !string.IsNullOrWhiteSpace(r.TenXa))
                    .GroupBy(r => r.TenTinh.Trim())
                    .Select(g => new ProvinceImportDto
                    {
                        Name = g.Key,
                        Description = "",
                        Wards = g.Select(w => new WardImportDto
                        {
                            Name = w.TenXa.Trim(),
                            Description = "",
                        }).ToList()
                    })
                    .ToList();

                if (!provinceDtos.Any())
                    throw new Exception("No valid province and ward data found in JSON.");

                foreach (var provinceDto in provinceDtos)
                {
                    if (string.IsNullOrWhiteSpace(provinceDto.Name))
                        continue;

                    var existingProvince = await FindOrCreateProvinceAsync(provinceDto, now);

                    foreach (var wardDto in provinceDto.Wards)
                    {
                        if (string.IsNullOrWhiteSpace(wardDto.Name))
                            continue;

                        await AddWardIfNotExistsAsync(wardDto, existingProvince.ID_Province, now);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error parsing JSON: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Database error while importing data: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
        }
        private async Task<PROVINCE> GetProvinceByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new Exception("Province name is invalid.");
                var province = await _context.PROVINCEs.FirstOrDefaultAsync(p => p.Name == name);
                return province;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving province by name: {ex.Message}");
            }
        }

        private async Task<PROVINCE> CreateProvinceAsync(ProvinceImportDto provinceDto, DateTime now)
        {
            try
            {
                if (provinceDto == null || string.IsNullOrWhiteSpace(provinceDto.Name))
                    throw new Exception("Province data is invalid.");

                var province = new PROVINCE
                {
                    Name = provinceDto.Name,
                    Description = provinceDto.Description,
                    CreateDate = now,
                };
                _context.PROVINCEs.Add(province);
                await _context.SaveChangesAsync();
                return province;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating province: {ex.Message}");
            }
        }

        private async Task<PROVINCE> FindOrCreateProvinceAsync(ProvinceImportDto provinceDto, DateTime now)
        {
            try
            {
                if (provinceDto == null || string.IsNullOrWhiteSpace(provinceDto.Name))
                    throw new Exception("Province data is invalid.");

                var existingProvince = await GetProvinceByNameAsync(provinceDto.Name);

                if (existingProvince == null)
                {
                    existingProvince = await CreateProvinceAsync(provinceDto, now);
                }

                return existingProvince;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error finding or creating province: {ex.Message}");
            }
        }

        private async Task AddWardIfNotExistsAsync(WardImportDto wardDto, int provinceId, DateTime now)
        {
            try
            {
                if (wardDto == null || string.IsNullOrWhiteSpace(wardDto.Name))
                    throw new Exception("Ward data is invalid.");

                var existsWard = await _context.WARDs.AnyAsync(w =>
                    w.Name == wardDto.Name && w.ID_Province == provinceId);

                if (existsWard)
                    return;

                var ward = new WARD
                {
                    Name = wardDto.Name,
                    Description = wardDto.Description,
                    ID_Province = provinceId,
                    CreateDate = now,
                };

                _context.WARDs.Add(ward);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding ward: {ex.Message}");
            }
        }

        public async Task<ProvinceDto> GetProvinceByIdAsync(int id)
        {
            try
            {
                var province = await _context.PROVINCEs
                    .Include(p => p.WARDs)
                    .FirstOrDefaultAsync(p => p.ID_Province == id);

                if (province == null)
                    throw new Exception($"Province with ID {id} not found.");

                var provinceDto = new ProvinceDto
                {
                    ID_Province = province.ID_Province,
                    Name = province.Name,
                    Description = province.Description,
                    Wards = province.WARDs.Select(w => new WardDto
                    {
                        ID_Ward = w.ID_Ward,
                        Name = w.Name,
                        Description = w.Description,
                        ID_Province = w.ID_Province
                    }).ToList()
                };

                return provinceDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving province by ID: {ex.Message}");
            }
        }
    }

}