using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("INVOICE_DETAIL")]
public partial class INVOICE_DETAIL
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_InvoiceDetail { get; set; }

    [Required]
    public int ID_Invoice { get; set; }

    [Required]
    public int ID_Product { get; set; }

    [Required]
    public int Count { get; set; }

    [Required]
    public decimal Price { get; set; }

    [ForeignKey(nameof(ID_Invoice))]
    public virtual INVOICE InvoiceNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Product))]
    public virtual PRODUCT ProductNavigation { get; set; } = null!;
}
