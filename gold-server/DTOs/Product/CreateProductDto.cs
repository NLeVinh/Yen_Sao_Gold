using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int? ID_Category { get; set; }

        [Required]
        public int ID_Status { get; set; }
    }
}