using System;
using System.Collections.Generic;

namespace WepTradeApp.Entities;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
