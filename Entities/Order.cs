using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int StatusId { get; set; }

    public int PickuppointId { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

 

    public string? Username { get; set; }

    public int GetCode { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PickupPoint Pickuppoint { get; set; } = null!;

    public virtual Status Status { get; set; } = new Status();

    public virtual User? UsernameNavigation { get; set; }
}
