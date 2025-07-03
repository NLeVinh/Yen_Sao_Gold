using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gold_server.Models;

[Table("BANNERS")]
public partial class BANNER
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_Banner { get; set; }

    [Required()]
    [MaxLength(255)]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime CreateDate { get; set; }

    [Required]
    public int CreateBy { get; set; }

    [ForeignKey(nameof(CreateBy))]
    public virtual USER CreateByNavigation { get; set; } = null!;

    public virtual ICollection<BANNER_IMAGE> BANNER_IMAGEs { get; set; } = new List<BANNER_IMAGE>();
}
