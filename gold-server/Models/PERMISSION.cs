using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("PERMISSIONS")]
public partial class PERMISSION
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Permission { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    public virtual ICollection<ROLE> ROLEs { get; set; } = new List<ROLE>();
}
