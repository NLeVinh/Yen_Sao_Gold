using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class PAYMENT_METHOD
{
    public int ID_Payment { get; set; }

    public string PaymentName { get; set; } = null!;

    public virtual ICollection<INVOICE> INVOICEs { get; set; } = new List<INVOICE>();
}
