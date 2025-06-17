using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class STATUS
{
    public int ID_Status { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<INVOICE> INVOICEs { get; set; } = new List<INVOICE>();

    public virtual ICollection<PRODUCT> PRODUCTs { get; set; } = new List<PRODUCT>();
}
