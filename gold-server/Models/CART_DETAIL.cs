using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class CART_DETAIL
{
    public int ID_CartDetail { get; set; }

    public int ID_Cart { get; set; }

    public string ID_Product { get; set; } = null!;

    public int Count { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual CART ID_CartNavigation { get; set; } = null!;

    public virtual PRODUCT ID_ProductNavigation { get; set; } = null!;
}
