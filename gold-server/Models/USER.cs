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

    public virtual ROLE ID_RoleNavigation { get; set; } = null!;
}
