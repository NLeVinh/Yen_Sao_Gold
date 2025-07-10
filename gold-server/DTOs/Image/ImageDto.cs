using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace gold_server.DTOs.Image
{
    public class ImageDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Image { get; set; }
        [Required]
        [MaxLength(1000)]
        public string URL { get; set; } = string.Empty;
    }
}