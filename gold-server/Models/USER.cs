using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace gold_server.Models;

[Table("USERS")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Phone), IsUnique = true)]
public partial class USER
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_User { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Password_Hash { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = null!;

    [Required]
    public int ID_Role { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    [ForeignKey(nameof(ID_Role))]
    public virtual ROLE RoleNavigation { get; set; } = null!;

    public virtual ICollection<CART_DETAIL> CART_DETAILs { get; set; } = new List<CART_DETAIL>();
    public virtual ICollection<BANNER> CREATED_BANNERs { get; set; } = new List<BANNER>();
    public virtual ICollection<CATEGORY> CREATED_CATEGORIEs { get; set; } = new List<CATEGORY>();
    public virtual ICollection<CATEGORY> UPDATED_CATEGORIEs { get; set; } = new List<CATEGORY>();
    public virtual ICollection<INVOICE> INVOICEs { get; set; } = null!;
    public virtual ICollection<PRODUCT> CREATED_PRODUCTs { get; set; } = new List<PRODUCT>();
    public virtual ICollection<PRODUCT> UPDATED_PRODUCTs { get; set; } = new List<PRODUCT>();
}
