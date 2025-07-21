using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gold_server.DTOs.Image;
using gold_server.DTOs.Product;

namespace gold_server.DTOs.Category
{
    public class CategoryDto
    {
        public int ID_Category { get; set; }
        public string Name { get; set; } = null!;
        public int CreateBy { get; set; }
        public List<ProductDto> Products { get; set; } = new();
        public List<ImageDto> Images { get; set; } = new();
    }
}