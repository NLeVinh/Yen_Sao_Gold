using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("PAYMENT_METHOD")]
public partial class PAYMENT_METHOD
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Payment { get; set; }

    [Required]
    [MaxLength(100)]
    public string PaymentName { get; set; } = null!;

    public virtual ICollection<INVOICE> INVOICEs { get; set; } = new List<INVOICE>();
}
