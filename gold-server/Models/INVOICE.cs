using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class INVOICE
{
    public int ID_Invoice { get; set; }

    public string? ID_User { get; set; }

    public int ID_Payment { get; set; }

    public decimal? Total { get; set; }

    public DateTime CreateDate { get; set; }

    public int? ID_Status { get; set; }

    public string Address { get; set; } = null!;

    public virtual PAYMENT_METHOD ID_PaymentNavigation { get; set; } = null!;

    public virtual STATUS? ID_StatusNavigation { get; set; }

    public virtual USER? ID_UserNavigation { get; set; }

    public virtual ICollection<INVOICE_DETAIL> INVOICE_DETAILs { get; set; } = new List<INVOICE_DETAIL>();
}
