using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class REF_IMAGE
{
    public int ID_Ref { get; set; }

    public string ID_ImageRef { get; set; } = null!;

    public int ID_Image { get; set; }

    public string? RefType { get; set; }

    public virtual IMAGE ID_ImageNavigation { get; set; } = null!;
}
