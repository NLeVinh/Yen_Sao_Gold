using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace gold_server.DTOs.Product
{
    public class UpdateProductDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? ID_Category { get; set; }

        public int? InStock { get; set; }
        public int? SoldQuantity { get; set; }

        public int? ID_Status { get; set; }

        /// <summary>
        /// List of IMAGE IDs the client wants to KEEP.
        /// </summary>
        public List<int>? ImagesToKeep { get; set; }

        /// <summary>
        /// List of new image files to upload.
        /// </summary>
        public List<IFormFile>? NewImages { get; set; }
    }
}