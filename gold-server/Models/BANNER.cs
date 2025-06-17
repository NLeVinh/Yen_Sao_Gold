using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class BANNER
{
    public int IndexAutoBanner { get; set; }

    public string ID_Banner { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public virtual USER CreateByNavigation { get; set; } = null!;
}
