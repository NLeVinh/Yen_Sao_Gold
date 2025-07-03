using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("CATEGORIES")]
public partial class CATEGORY
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Category { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    [Required]
    public int CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    [ForeignKey(nameof(CreateBy))]
    public virtual USER? CreateByNavigation { get; set; }

    [ForeignKey(nameof(UpdateBy))]
    public virtual USER? UpdateByNavigation { get; set; }

    public virtual ICollection<PRODUCT> PRODUCTs { get; set; } = new List<PRODUCT>();

    public virtual ICollection<CATEGORY_IMAGE> CATEGORY_IMAGEs { get; set; } = new List<CATEGORY_IMAGE>();
}
