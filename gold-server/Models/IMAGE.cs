using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("IMAGES")]
public partial class IMAGE
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Image { get; set; }

    [Required]
    [MaxLength(1000)]
    public string URL { get; set; } = null!;

    public virtual ICollection<PRODUCT_IMAGE> PRODUCT_IMAGEs { get; set; } = new List<PRODUCT_IMAGE>();

    public virtual ICollection<BANNER_IMAGE> BANNER_IMAGEs { get; set; } = new List<BANNER_IMAGE>();

    public virtual ICollection<CATEGORY_IMAGE> CATEGORY_IMAGEs { get; set; } = new List<CATEGORY_IMAGE>();
}

[Table("PRODUCT_IMAGE")]
public partial class PRODUCT_IMAGE
{
    [Key]
    [Column(Order = 0)]
    public int ID_Image { get; set; }

    [Key]
    [Column(Order = 1)]
    public int ID_Product { get; set; }

    [Required]
    public int SortOrder { get; set; }

    [MaxLength(100)]
    public string? AltText { get; set; }

    [ForeignKey(nameof(ID_Image))]
    public virtual IMAGE ImageNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Product))]
    public virtual PRODUCT ProductNavigation { get; set; } = null!;
}

[Table("BANNER_IMAGE")]
public partial class BANNER_IMAGE
{
    [Key]
    [Column(Order = 0)]
    public int ID_Image { get; set; }

    [Key]
    [Column(Order = 1)]
    public int ID_Banner { get; set; }

    [Required]
    public int SortOrder { get; set; }

    [MaxLength(100)]
    public string? AltText { get; set; }

    [ForeignKey(nameof(ID_Image))]
    public virtual IMAGE ImageNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Banner))]
    public virtual BANNER BannerNavigation { get; set; } = null!;
}

[Table("CATEGORY_IMAGE")]
public partial class CATEGORY_IMAGE
{
    [Key]
    [Column(Order = 0)]
    public int ID_Image { get; set; }

    [Key]
    [Column(Order = 1)]
    public int ID_Category { get; set; }

    [Required]
    public int SortOrder { get; set; }

    [MaxLength(100)]
    public string? AltText { get; set; }

    [ForeignKey(nameof(ID_Image))]
    public virtual IMAGE ImageNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Category))]
    public virtual CATEGORY CategoryNavigation { get; set; } = null!;
}
