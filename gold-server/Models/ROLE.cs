using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class ROLE
{
    public int ID_Roles { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<USER> USERs { get; set; } = new List<USER>();

    public virtual ICollection<PERMISSION> ID_Permissions { get; set; } = new List<PERMISSION>();
}
