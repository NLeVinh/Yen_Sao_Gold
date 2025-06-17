using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class INVOICE_DETAIL
{
    public int ID_InvoiceDetail { get; set; }

    public int ID_Invoice { get; set; }

    public string ID_Product { get; set; } = null!;

    public int Count { get; set; }

    public decimal Price { get; set; }

    public virtual INVOICE ID_InvoiceNavigation { get; set; } = null!;

    public virtual PRODUCT ID_ProductNavigation { get; set; } = null!;
}
