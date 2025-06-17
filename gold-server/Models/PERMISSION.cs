using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class PERMISSION
{
    public int ID_Permission { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ROLE> ID_Roles { get; set; } = new List<ROLE>();
}
