using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.Province
{
    public class ProvinceImportDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
        public int ID_Status { get; set; }

        public List<WardImportDto> Wards { get; set; } = new List<WardImportDto>();
    }
}