using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class IMAGE
{
    public int ID_Image { get; set; }

    public string URL { get; set; } = null!;

    public virtual ICollection<REF_IMAGE> REF_IMAGEs { get; set; } = new List<REF_IMAGE>();
}
