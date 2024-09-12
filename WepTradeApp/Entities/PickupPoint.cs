using System;
using System.Collections.Generic;

namespace WepTradeApp.Entities;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
