using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [MaxLength(255)]
        public string? Name { get; set; }

        public List<int>? ImagesToKeep { get; set; }
        public List<IFormFile>? NewImages { get; set; }
    }
}