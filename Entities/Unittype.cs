using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

public partial class Unittype
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
