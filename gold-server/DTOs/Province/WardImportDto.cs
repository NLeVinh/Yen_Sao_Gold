using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.Province
{
    public class WardImportDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public int ID_Province { get; set; }
        public int ID_Status { get; set; }
    }
}