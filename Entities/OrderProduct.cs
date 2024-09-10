using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

public partial class OrderProduct
{
    public string ProductId { get; set; } = null!;

    public int OrderId { get; set; }

    public int? Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
