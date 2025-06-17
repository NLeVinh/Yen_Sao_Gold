using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class CART
{
    public int ID_Cart { get; set; }

    public string ID_User { get; set; } = null!;

    public virtual ICollection<CART_DETAIL> CART_DETAILs { get; set; } = new List<CART_DETAIL>();

    public virtual USER ID_UserNavigation { get; set; } = null!;
}
