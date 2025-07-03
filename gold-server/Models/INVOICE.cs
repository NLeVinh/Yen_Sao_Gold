using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("INVOICES")]
public partial class INVOICE
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Invoice { get; set; }

    public int? ID_User { get; set; }

    [Required]
    public int ID_Payment { get; set; }

    [Required]
    public decimal Total { get; set; } = 0;

    [Required]
    public DateTime CreateDate { get; set; }

    [Required]
    public int ID_Status { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName_Receiver { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    public string Phone_Receiver { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;

    [MaxLength(255)]
    public string? Note { get; set; }

    [ForeignKey(nameof(ID_Payment))]
    public virtual PAYMENT_METHOD PaymentNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_Status))]
    public virtual STATUS StatusNavigation { get; set; } = null!;

    [ForeignKey(nameof(ID_User))]
    public virtual USER? UserNavigation { get; set; }

    public virtual ICollection<INVOICE_DETAIL> INVOICE_DETAILs { get; set; } = new List<INVOICE_DETAIL>();
}
