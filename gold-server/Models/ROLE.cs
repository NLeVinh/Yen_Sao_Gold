using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("ROLES")]
public partial class ROLE
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Role { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    public virtual ICollection<USER> USERs { get; set; } = new List<USER>();

    public virtual ICollection<PERMISSION> PERMISSIONs { get; set; } = new List<PERMISSION>();
}
