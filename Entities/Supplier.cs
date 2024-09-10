using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
