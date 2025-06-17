using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class CATEGORy
{
    public int IndexAutoCategory { get; set; }

    public string ID_Category { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public virtual USER? CreateByNavigation { get; set; }

    public virtual ICollection<PRODUCT> PRODUCTs { get; set; } = new List<PRODUCT>();

    public virtual USER? UpdateByNavigation { get; set; }
}
