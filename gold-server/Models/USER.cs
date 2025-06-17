using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class USER
{
    public int IndexAutoUser { get; set; }

    public string ID_User { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Password_Hash { get; set; }

    public string Phone { get; set; } = null!;

    public int ID_Role { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<BANNER> BANNERs { get; set; } = new List<BANNER>();

    public virtual ICollection<CART> CARTs { get; set; } = new List<CART>();

    public virtual ICollection<CATEGORy> CATEGORyCreateByNavigations { get; set; } = new List<CATEGORy>();

    public virtual ICollection<CATEGORy> CATEGORyUpdateByNavigations { get; set; } = new List<CATEGORy>();

    public virtual ROLE ID_RoleNavigation { get; set; } = null!;

    public virtual ICollection<INVOICE> INVOICEs { get; set; } = new List<INVOICE>();

    public virtual ICollection<PRODUCT> PRODUCTCreateByNavigations { get; set; } = new List<PRODUCT>();

    public virtual ICollection<PRODUCT> PRODUCTUpdateByNavigations { get; set; } = new List<PRODUCT>();
}
