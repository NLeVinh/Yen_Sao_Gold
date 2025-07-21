using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace gold_server.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int? ID_Category { get; set; }

        [Required]
        public int ID_Status { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}