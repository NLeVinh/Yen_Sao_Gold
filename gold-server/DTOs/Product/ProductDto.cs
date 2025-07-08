using System;

namespace gold_server.DTOs.Product
{
    public class ProductDto
    {
        public int ID_Product { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? ID_Category { get; set; }
        public int InStock { get; set; }
        public int SoldQuantity { get; set; }
        public int ID_Status { get; set; }
    }
}