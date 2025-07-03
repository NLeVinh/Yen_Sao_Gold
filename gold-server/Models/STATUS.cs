using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("STATUS")]
public partial class STATUS
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Status { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    public virtual ICollection<INVOICE> INVOICEs { get; set; } = new List<INVOICE>();

    public virtual ICollection<PRODUCT> PRODUCTs { get; set; } = new List<PRODUCT>();
}
