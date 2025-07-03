using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("PRODUCTS")]
public partial class PRODUCT
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Product { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    public int? ID_Category { get; set; }

    public int? UpdateBy { get; set; }

    [Required]
    public int CreateBy { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    [Required]
    public int InStock { get; set; } = 0;

    [Required]
    public int ID_Status { get; set; }

    [Required]
    public int SoldQuantity { get; set; } = 0;

    [ForeignKey(nameof(CreateBy))]
    public virtual USER CreateByNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Category))]
    public virtual CATEGORY? CategoryNavigation { get; set; }

    [ForeignKey(nameof(ID_Status))]
    public virtual STATUS? StatusNavigation { get; set; }

    [ForeignKey(nameof(UpdateBy))]
    public virtual USER UpdateByNavigation { get; set; } = null!;
    
    public virtual ICollection<PRODUCT_IMAGE> PRODUCT_IMAGEs { get; set; } = new List<PRODUCT_IMAGE>();

    public virtual ICollection<CART_DETAIL> CART_DETAILs { get; set; } = new List<CART_DETAIL>();

    public virtual ICollection<INVOICE_DETAIL> INVOICE_DETAILs { get; set; } = new List<INVOICE_DETAIL>();
}
