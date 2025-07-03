using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("CART_DETAIL")]
public partial class CART_DETAIL
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_CartDetail { get; set; }

    [Required]
    public int ID_User { get; set; }

    [Required]
    public int ID_Product { get; set; }

    [Required]
    public int Count { get; set; }

    public DateTime? UpdateDate { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    [ForeignKey(nameof(ID_User))]
    public virtual USER UserNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Product))]
    public virtual PRODUCT ProductNavigation { get; set; } = null!;
}
