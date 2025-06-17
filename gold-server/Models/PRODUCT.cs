using System;
using System.Collections.Generic;

namespace gold_server.Models;

public partial class PRODUCT
{
    public int IndexAutoProduct { get; set; }

    public string ID_Product { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ID_Category { get; set; }

    public string UpdateBy { get; set; } = null!;

    public string CreateBy { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? InStock { get; set; }

    public int? ID_Status { get; set; }

    public int? SoldQuantity { get; set; }

    public virtual ICollection<CART_DETAIL> CART_DETAILs { get; set; } = new List<CART_DETAIL>();

    public virtual USER CreateByNavigation { get; set; } = null!;

    public virtual CATEGORy? ID_CategoryNavigation { get; set; }

    public virtual STATUS? ID_StatusNavigation { get; set; }

    public virtual ICollection<INVOICE_DETAIL> INVOICE_DETAILs { get; set; } = new List<INVOICE_DETAIL>();

    public virtual USER UpdateByNavigation { get; set; } = null!;
}
